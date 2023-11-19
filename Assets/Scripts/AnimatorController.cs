using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public GameObject Trump;


    private float length = 0;
    private bool canWait = true;
    private bool canPlay = true;


    public AudioPlayer audioPlayer;
    // Update is called once per frame
    private void Update()
    {
        Debug.Log(canPlay);
        if ((String.Equals(OpenAI.animationEvent, "talking") || (String.Equals(OpenAI.animationEvent, "disappointed_laydown"))) && canWait && audioPlayer.audioSource.clip.length != 0)
        {
            canWait = false;
            length = audioPlayer.audioSource.clip.length;
            gameObject.GetComponent<Animator>().SetBool("talking", true);
            StartCoroutine(PlayTalkingAnimation(length));
        }

        if (canPlay) {
            
            if (String.Equals(OpenAI.animationEvent, "waving"))
            {
                canPlay = false;
                gameObject.GetComponent<Animator>().SetBool("talking", false);
                gameObject.GetComponent<Animator>().SetTrigger("waving");
                length = audioPlayer.audioSource.clip.length;
                StartCoroutine(PlayTalkingAnimation(length));
            }

            if (String.Equals(OpenAI.animationEvent, "shouting"))
            {
                canPlay = false;
                gameObject.GetComponent<Animator>().SetBool("talking", false);
                gameObject.GetComponent<Animator>().SetTrigger("shouting");
                length = audioPlayer.audioSource.clip.length;
                StartCoroutine(PlayTalkingAnimation(length));
            }

       

            if (String.Equals(OpenAI.animationEvent, "shoulder_shrugging"))
            {
                canPlay = false;
                gameObject.GetComponent<Animator>().SetBool("talking", false);
                gameObject.GetComponent<Animator>().SetTrigger("shoulder_shrugging");
                length = audioPlayer.audioSource.clip.length;
                StartCoroutine(PlayTalkingAnimation(length));
            }

        }


    }

    private IEnumerator PlayTalkingAnimation(float duration)
    {
        gameObject.GetComponent<Animator>().SetBool("idle", false);
        yield return new WaitForSeconds(duration); // Wait for the duration of the audio clip

        canWait = true;
        canPlay = true;

        gameObject.GetComponent<Animator>().SetBool("talking", false);
        if (String.Equals(OpenAI.animationEvent, "disappointed_laydown"))
        {
            gameObject.GetComponent<Animator>().SetTrigger("disappointed_laydown");
        }
        else
        {
            OpenAI.animationEvent = "idle";
            gameObject.GetComponent<Animator>().SetBool("idle", true);
        }

        

    }


    /*
    private async Task WaitAsync()
    {
        
        Debug.Log(length);




        if (String.Equals(OpenAI.animationEvent, "disappointed_laydown"))
        {
            gameObject.GetComponent<Animator>().SetTrigger("disappointed_laydown");
        }
        else
        {
            gameObject.GetComponent<Animator>().SetTrigger("idle");
        }
        canWait = true;
        Debug.Log("bbbb");
    }*/

}