using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    private void Update()
    {
        if(OpenAI.validResponse && OpenAI.audioLink != "")
        {
            DownloadAndPlayAudio(OpenAI.audioLink);
            OpenAI.validResponse = false;
        }
    }

    private async Task DownloadAndPlayAudio(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            var operation = www.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Delay(100); // Wait for a short period before checking again
            }

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
}
