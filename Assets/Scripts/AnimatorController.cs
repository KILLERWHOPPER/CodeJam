using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public GameObject RightArm;
    public GameObject LeftArm;
    public GameObject Head;
    public GameObject RightLeg;
    public GameObject LeftLeg;
    public GameObject Kevin;

    private float length = 10;
    private bool canTalk = false;

    //public TextToSpeech textToSpeech;
    // Update is called once per frame
    void Update()
    {
        //length = textToSpeech.audioSource.clip.length;

        if (canTalk) {
            SetAnimation("blabling", "blabling", "blabling", null, null, null);
        }
        else {

            if (Input.GetKeyDown("d")){
                SetAnimation(null, null, "wave");
            }
            if (Input.GetKeyDown("a")) {
                SetAnimation(null, "jump", "jump", "jump", "jump", "jump");
            }
            if (Input.GetKeyDown("w")) {
                StartCoroutine(WaitForTalk());
                Debug.Log("coroutine");
            }




        }

    }

    private void SetAnimation(string headtrigger = null, string larmtrigger = null, string rarmtrigger = null, string kevintrigger = null, string llegtrigger = null, string rlegtrigger = null) {
        Head.GetComponent<Animator>().SetTrigger(headtrigger);
        RightArm.GetComponent<Animator>().SetTrigger(rarmtrigger);
        LeftArm.GetComponent<Animator>().SetTrigger(larmtrigger);
        Kevin.GetComponent<Animator>().SetTrigger(kevintrigger);
        RightLeg.GetComponent<Animator>().SetTrigger(rlegtrigger);
        LeftLeg.GetComponent<Animator>().SetTrigger(llegtrigger);
    }
    private IEnumerator WaitForTalk() {
        canTalk = true;
        Debug.Log("stated talk");
        yield return new WaitForSeconds(length);
        Debug.Log("finished talk");
        canTalk = false;
    }
}
