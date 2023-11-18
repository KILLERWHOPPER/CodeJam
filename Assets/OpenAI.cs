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

    public class ErrorResponse
    {
        public string error;
    }

    string Base = "http://10.75.0.150:5000";
    string animation = "";
    string dialogue = "";

    IEnumerator SoundAsync()
    {
        string url = Base + "/";
        RequestMicrophoneData data = new RequestMicrophoneData();
        data.dialogue = dialogue;

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
            }
            else
            {
                Debug.Log("response received for " + dialogue + ", loading image");
                string response = request.downloadHandler.text;
                byte[] imageData = Convert.FromBase64String(response);

                //do stuff with data
            }
        }
    }

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
