using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceObj : Evidence, Interactable
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
