using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeycodeInteract : MonoBehaviour, Interactable
{
    [SerializeField] int code;
    [SerializeField] UnityEvent onCodeEntered;

    bool completed = false;

    public IEnumerator Interact()
    {
        yield return GameController.i.StateMachine.PushAndWait(KeycodeState.i);

        if (KeycodeState.i.Submitted)
        {
            if (code == KeycodeState.i.CurrentCode && completed == false)
            {
                Reliability.i.AffectReliability(10);
                completed = true;
                onCodeEntered.Invoke();
            }
            else if(code != KeycodeState.i.CurrentCode)
                Reliability.i.AffectReliability(-7);
        }
    }
}
