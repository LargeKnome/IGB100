using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockInteract : MonoBehaviour, Interactable
{
    [SerializeField] Key requiredEvidence;
    [SerializeField] UnityEvent onUnlocked;

    private bool completed = false;

    Material defaultMat;

    private void Awake()
    {
        defaultMat = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        GameController.i.Player.OnVisionActivate += UpdateMaterial;
    }

    void UpdateMaterial(bool activated)
    {
        gameObject.GetComponent<MeshRenderer>().material = (activated) ? GameController.i.Player.DetectivisionMat : defaultMat;
    }

    public IEnumerator Interact()
    {
        InventoryState.i.InteractWithLock = true;
        yield return GameController.i.StateMachine.PushAndWait(InventoryState.i);

        if (InventoryState.i.HasSelectedEvidence)
        {
            if (InventoryState.i.SelectedEvidence == requiredEvidence && completed == false)
            {
                Reliability.i.AffectReliability(10);
                completed = true;
                onUnlocked.Invoke();
            }
            else if (InventoryState.i.SelectedEvidence != requiredEvidence)
            {
                Reliability.i.AffectReliability(-7);
            }
        }
    }
}
