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
        public string dialogue;
    }

    class ResponseData
    {
        public string text;
        public string animation;
    }

    public class ErrorResponse
    {
        public string error;
    }

    string Base = "";

    public static bool validResponse = false;
    public static string animationEvent = "";
    public static string text = "";

    public void GenerateResponse(string prompt)
    {
        if (!string.IsNullOrEmpty(prompt))
        {
            StartCoroutine(SoundAsync(prompt));
        }
    }

    IEnumerator SoundAsync(string prompt)
    {
        string url = Base + "/";
        RequestMicrophoneData data = new RequestMicrophoneData();
        data.dialogue = prompt;

        string jsonData = JsonUtility.ToJson(data);
        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        Debug.Log("sending request...");
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(byteData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

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
                Debug.Log("response received for " + prompt);
                string response = request.downloadHandler.text;
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(response);

                text = responseData.text;
                animationEvent = responseData.animation;
                validResponse = true;
            }
        }
    }

}
