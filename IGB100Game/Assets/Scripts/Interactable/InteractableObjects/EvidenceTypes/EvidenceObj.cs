using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceObj : Evidence, Interactable
{
    protected override void OnStart()
    {
        defaultMat = GetComponent<MeshRenderer>().material;

        GameController.i.Player.OnVisionActivate += UpdateMaterial;
    }

    public IEnumerator Interact()
    {
        yield return OnPickup();
    }
}
