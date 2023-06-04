using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PoliceChief : MonoBehaviour, Interactable
{
    [SerializeField] Evidence motive;
    [SerializeField] Evidence weapon;
    [SerializeField] NPCController murderer;

    [SerializeField] UnityEvent OnSuccessfulAccusation;

    public IEnumerator Interact()
    {
        yield return DialogManager.i.ShowLine("What evidence do you have to show me?", false);

        yield return GameController.i.StateMachine.PushAndWait(AccusationState.i);

        if (!AccusationState.i.HasAccused)
            yield break;

        bool successfulAccusation = AccusationState.i.Suspect == murderer && AccusationState.i.Motive == motive && AccusationState.i.Weapon == weapon;

        if (successfulAccusation)
        {
            yield return DialogManager.i.ShowLine("Alright, I'm convinced. You solved the case, detective. well done.", false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            OnSuccessfulAccusation?.Invoke();
        }
        else
        {
            yield return DialogManager.i.ShowLine("I don't think I'm convinced.", false);
            Reliability.i.AffectReliability(-33);
        }
    }
}
