using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour {
    public GameObject Trump;


    private float length = 10;
    private bool talking = false;
    private bool canWait = true;

    //public TextToSpeech textToSpeech;
    // Update is called once per frame
    void Update() {
        //length = textToSpeech.audioSource.clip.length;

        if (Input.GetKeyDown("talking")) {
            if (canWait == true) {
                    canWait = false;
                Debug.Log("talking true");
                    StartCoroutine(Wait());
                Debug.Log("talking false");
            }
                

        }
    

        if (talking == true) {
            gameObject.GetComponent<Animator>().SetBool("talking", true);
        }
        if (talking == false) {
            gameObject.GetComponent<Animator>().SetBool("talking", false);
        }
        
        if (Input.GetKeyDown("waving")) {
            gameObject.GetComponent<Animator>().SetTrigger("waving");   

        }

        if (Input.GetKeyDown("shouting")) {
            gameObject.GetComponent<Animator>().SetTrigger("shouting");

        }

        if (Input.GetKeyDown("disappointed_laydown")) {
            gameObject.GetComponent<Animator>().SetTrigger("disappointed_laydown");

        }

        if (Input.GetKeyDown("shoulder_shrugging")) {
            gameObject.GetComponent<Animator>().SetTrigger("shoulder_shrugging");

        }






    }

    private IEnumerator Wait() {
        talking = true;
        Debug.Log("aaaa");
        yield return new WaitForSeconds(length);
        talking = false;
        canWait = true;
        Debug.Log("bbbb");
    }

}
