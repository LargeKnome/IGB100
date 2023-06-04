using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeycodeInteract : MonoBehaviour, Interactable
{
    [SerializeField] int code;
    [SerializeField] UnityEvent onCodeEntered;

    [SerializeField] AudioClip unlockSound;

    bool completed = false;

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
        yield return GameController.i.StateMachine.PushAndWait(KeycodeState.i);

        if (KeycodeState.i.Submitted)
        {
            if (code == KeycodeState.i.CurrentCode && completed == false)
            {
                Reliability.i.AffectReliability(10);
                completed = true;
                AudioManager.i.PlaySFX(unlockSound);
                onCodeEntered.Invoke();
            }
            else if(code != KeycodeState.i.CurrentCode)
                Reliability.i.AffectReliability(-7);
        }
    }
}
