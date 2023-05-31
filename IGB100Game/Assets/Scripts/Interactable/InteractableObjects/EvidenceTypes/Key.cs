using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Evidence, Interactable
{
    protected override void OnStart()
    {
        SetUpMats();
    }

    public IEnumerator Interact()
    {
        yield return OnPickup();
    }
}
