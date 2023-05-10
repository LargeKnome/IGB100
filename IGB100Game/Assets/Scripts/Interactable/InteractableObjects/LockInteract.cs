using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockInteract : MonoBehaviour, Interactable
{
    [SerializeField] Key requiredEvidence;
    [SerializeField] UnityEvent onUnlocked;

    private bool completed = false;

    public IEnumerator Interact()
    {
        if (requiredEvidence != null)
        {
            if (GameController.i.Player.Inventory.EvidenceList.Contains(requiredEvidence) && completed == false)
            {
                completed = true;
                onUnlocked.Invoke();
            }
        }
        
        yield return null;
    }
}
