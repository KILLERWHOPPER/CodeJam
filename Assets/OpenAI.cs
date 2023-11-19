using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAI : MonoBehaviour
{
    class RequestMicrophoneData
    {
        public string text_message;
    }

    class RequestChar
    {
        public string char_name;
        public string series;
    }

    class ResponseData
    {
        public string text_message;
        public string face_expression;
        public string body_movement;
        public string voice_link;
    }

    public class ErrorResponse
    {
        public string error;
    }

    string Base = "http://127.0.0.1:8000";

    public static bool validResponse = false;
    public static string animationEvent;
    public static string audioLink;
    public static string text = "";
    public static string faceExpression = "";

    private void Start()
    {
        string url = Base + "/initialize/";

        RequestChar data = new RequestChar();
        data.char_name = "Trump";
        data.series = "America";

        string jsonData = JsonUtility.ToJson(data);
        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(byteData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        StartCoroutine(receive(request));

    }

    public void GenerateResponse(string prompt)
    {
        if (!string.IsNullOrEmpty(prompt))
        {
            SoundAsync(prompt);
        }
    }

    void SoundAsync(string prompt)
    {
        string url = Base + "/receive_input/";
        RequestMicrophoneData data = new RequestMicrophoneData();
        data.text_message = prompt;

        string jsonData = JsonUtility.ToJson(data);
        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        Debug.Log("sending request...");
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(byteData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        StartCoroutine(receive(request));

    }

    IEnumerator receive(UnityWebRequest request)
    {
        using (request)
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                if (request.responseCode == 400)
                {
                    string response = request.downloadHandler.text;
                    ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(response);
                    Debug.LogError("Server Error: " + errorResponse.error);
                }
                else
                {
                    Debug.Log("Error: " + request.error);
                }
                validResponse = false;
            }
            else
            {
                string response = request.downloadHandler.text;
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(response);

                animationEvent = responseData.body_movement;
                text = responseData.text_message;
                audioLink = responseData.voice_link;
                faceExpression = responseData.face_expression;
                validResponse = true;
            }
        }
    }



}
