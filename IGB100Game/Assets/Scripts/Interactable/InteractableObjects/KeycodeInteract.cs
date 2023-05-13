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

        if(code == KeycodeState.i.CurrentCode && completed == false && KeycodeState.i.Submitted)
        {
            completed = true;
            onCodeEntered.Invoke();
        }
    }
}
