using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using System.IO;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class TextToSpeech : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Update()
    {
        if(OpenAI.validResponse)
        {
            playSound(OpenAI.text);
        }
    }

    private async void playSound(string text)
    {
        var credentials = new BasicAWSCredentials("", "");
        var client = new AmazonPollyClient(credentials, RegionEndpoint.USEast1);

        var request = new SynthesizeSpeechRequest()
        {
            Text = text,
            Engine = Engine.Neural,
            VoiceId = VoiceId.Arthur,
            OutputFormat = OutputFormat.Mp3
        };

        try
        {
            var response = await client.SynthesizeSpeechAsync(request);
            WriteIntoFile(response.AudioStream);
        }
        catch (Amazon.Runtime.Internal.HttpErrorResponseException ex)
        {
            Debug.Log(ex.Message);
        }


        using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3", AudioType.MPEG))
        {
            var op = www.SendWebRequest();
            while (!op.isDone)
            {
                await Task.Yield();
            }

            var clip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[4 * 1024];
            int bytesRead;

            while((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }

    public void timeTalking()
    {

    }
}
