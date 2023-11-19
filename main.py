import openai
from fastapi import FastAPI, HTTPException
from openai import OpenAI
from pydantic import BaseModel
import requests
import json
import fakeyou

app = FastAPI()
client = OpenAI(api_key="get your own key")
fakeyou_url = 'https://api.fakeyou.com/tts/inference'
fy = fakeyou.FakeYou()
fy.login("get your", "own account")
class CharacterInfo(BaseModel):
    char_name: str
    series: str
    convId: str | None = ""


class UserInput(BaseModel):
    text_message: str
    body_movement: str | None = "idle"


class VRCharacterResponse(BaseModel):
    text_message: str
    face_expression: str
    body_movement: str
    voice_link: str


convInfo = CharacterInfo
history = []
TTS_Book = {"Trump": "TM:03690khwpsbz"}

async def call_chatgpt(text_input: str, body_movement: str) -> str:
    try:

        input_json = json.dumps({"text": text_input, "body_movement": body_movement})
        hist = {"role": "user", "content": input_json}
        history.append(hist)
        gpt_response = client.chat.completions.create(
            model="gpt-3.5-turbo",
            messages=history,
        # conversation_id=convInfo.convId
        )
        print(gpt_response)
        hist2 = {"role": "assistant", "content": gpt_response.choices[0].message.content}
        history.append(hist2)
        return gpt_response.choices[0].message.content

    except Exception as e:
        raise HTTPException(status_code=500, detail="Error in call_chatgpt")

async def initGPTConv(char_name: str, from_series: str) -> str:
    try:
        text_input = "You are {__character} from {__series}. You have to respond and answer like {__character} using the tone, manner and vocabulary {__character} would use. Do not write any explanations. Only answer like {__character}. The answer should be in json format and include text answer 'text', face expression 'face_expression', and body movement 'body_movement'. The face expression should be one of: [normal, happy, sad, angry, confused, shocked]. The body movement should be one of: [talking, shouting, shoulder_shrugging, waving, disappointed_laydown]. If you understand, return your first respond as the character in json format.".format(
            __character=char_name, __series=from_series)
        input_json = json.dumps({"text": text_input, "body_movement": "idle"})
        hist = {"role": "system", "content": input_json}
        history.append(hist)
        print(history)
        gpt_response = client.chat.completions.create(
            model="gpt-3.5-turbo",
            messages=[{"role": "system", "content": input_json}],
            # conversation_id=convInfo.convId
        )
        print(gpt_response)
        convInfo.convId = gpt_response.id
        convInfo.char_name = char_name
        convInfo.series = from_series
        hist2 = {"role": "assistant", "content": gpt_response.choices[0].message.content}
        history.append(hist2)
        return gpt_response.choices[0].message.content

    except Exception as e:
        raise HTTPException(status_code=500, detail="Error in initGPTConv")


@app.post("/initialize/", response_model=VRCharacterResponse)
async def initialize(iniInfo: CharacterInfo):
    try:
        history.clear()
        response = await initGPTConv(iniInfo.char_name, iniInfo.series)

        res_json = json.loads(response)
        return VRCharacterResponse(
            text_message=res_json['text'],
            face_expression=res_json['face_expression'],
            body_movement=res_json['body_movement'],
            voice_link= await TTS(res_json['text'], convInfo.char_name)
        )


    except Exception as e:
        raise HTTPException(status_code=500, detail="Error in initialize")


@app.post("/receive_input/", response_model=VRCharacterResponse)
async def receive_input(user_input: UserInput):
    try:
        print(user_input)
        print(user_input.text_message)
        print(user_input.body_movement)
        response = await call_chatgpt(user_input.text_message, user_input.body_movement)
        res_json = json.loads(response)
        return VRCharacterResponse(
            text_message=res_json['text'],
            face_expression=res_json['face_expression'],
            body_movement=res_json['body_movement'],
            voice_link= await TTS(res_json['text'], convInfo.char_name)
        )

    except Exception as e:
        raise HTTPException(status_code=500, detail="error in receive_input")


@app.post("/TTS/")
async def TTS(text: str, character: str):


    try:
        ijt = fy.make_tts_job(text, TTS_Book[character])
        wav = fy.tts_poll(ijt)

        return wav.link
    except Exception as e:
        raise HTTPException(status_code=500, detail="error in TTS")
