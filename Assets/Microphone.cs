using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class Microphone : MonoBehaviour
{
    public InputActionReference gripInputActionReference;
    //public MicrophoneRecord microphoneRecord;
    private void OnEnable()
    {
        gripInputActionReference.action.started += OnGripButtonPressed;
    }

    private void OnDisable()
    {
        gripInputActionReference.action.started -= OnGripButtonPressed;
    }

    private void OnGripButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("pressed");
        /*
        if (!microphoneRecord.IsRecording)
        { 
            microphoneRecord.StartRecord();
            buttonText.text = "Stop";
        }
        else
        {
            microphoneRecord.StopRecord();
            buttonText.text = "Record";
        }*/
    }
}
