using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TextToSpeech : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Update()
    {
        if(OpenAI.validResponse)
        {
            StartCoroutine(DownloadAndPlayAudio(OpenAI.audioLink));
        }
    }

    IEnumerator DownloadAndPlayAudio(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);

                audioSource.clip = audioClip;

                audioSource.Play();
            }
            else
            {
                Debug.LogError("Failed to download audio file. Error: " + www.error);
            }
        }
    }
}
