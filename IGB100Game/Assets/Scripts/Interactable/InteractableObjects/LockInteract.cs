using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockInteract : MonoBehaviour, Interactable
{
    [SerializeField] Key requiredEvidence;
    [SerializeField] UnityEvent onUnlocked;

    [SerializeField] AudioClip unlockSound;

    private bool completed = false;

    List<Material> defaultMats;

    private void Awake()
    {
        defaultMats = new List<Material>();

        foreach (Transform child in transform)
            defaultMats.Add(child.GetComponent<MeshRenderer>().material);
    }

    private void Start()
    {
        GameController.i.Player.OnVisionActivate += UpdateMaterial;
    }

    void UpdateMaterial(bool activated)
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material = (activated) ? GameController.i.Player.DetectivisionMat : defaultMats[i];
            i++;
        }
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
                AudioManager.i.PlaySFX(unlockSound);
                onUnlocked.Invoke();
            }
            else if (InventoryState.i.SelectedEvidence != requiredEvidence)
                Reliability.i.AffectReliability(-7);
        }
    }
}
