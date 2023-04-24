using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockInteract : MonoBehaviour, Interactable
{
    [SerializeField] Evidence requiredEvidence;
    [SerializeField] UnityEvent onUnlocked;

    public IEnumerator Interact()
    {

        if (requiredEvidence != null)
        {
            if (!GameController.i.Player.Inventory.EvidenceList.Contains(requiredEvidence))
                onUnlocked?.Invoke();
        }
        yield return null;
    }
}
