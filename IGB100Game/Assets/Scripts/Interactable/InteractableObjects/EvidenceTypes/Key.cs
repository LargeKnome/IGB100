using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Evidence, Interactable
{
    protected override void OnStart()
    {
        defaultMat = GetComponent<MeshRenderer>().material;

        GameController.i.Player.OnVisionActivate += UpdateMaterial;
    }

    public IEnumerator Interact()
    {
        foreach (string line in itemDescription)
            yield return DialogManager.i.ShowLine(line);
        yield return DialogManager.i.ShowLine("I better take this with me.");
        GameController.i.Player.Inventory.AddEvidence(this);

        gameObject.SetActive(false);
        yield return null;
    }
}
