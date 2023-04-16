using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evidence : MonoBehaviour, Interactable
{
    [SerializeField] string itemDescription;

    public string Name => name;
    public string Description => itemDescription;

    public IEnumerator Interact()
    {
        yield return DialogManager.i.ShowText(itemDescription);
        yield return DialogManager.i.ShowText("I better take this with me.");
        GameController.i.Player.Inventory.AddEvidence(this);
        gameObject.SetActive(false);
        yield return null;
    }
}
