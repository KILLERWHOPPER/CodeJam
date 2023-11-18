using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Poke : MonoBehaviour
{
    public Transform PokeAttachPoint;
    private XRPokeInteractor xrPoke;
    // Start is called before the first frame update
    void Start()
    {
        xrPoke = transform.parent.parent.GetComponentInChildren<XRPokeInteractor>();
        SetPokeAttachPoint();
    }

    void SetPokeAttachPoint()
    {
        if(PokeAttachPoint == null) { Debug.Log("Poke Attach Point is null"); return; }
        if(xrPoke == null) { Debug.Log("XR poke interactor is null"); return; }
        xrPoke.attachTransform = PokeAttachPoint;
    }
}
