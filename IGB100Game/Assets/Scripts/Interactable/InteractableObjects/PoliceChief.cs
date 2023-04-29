using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PoliceChief : MonoBehaviour, Interactable
{
    [SerializeField] List<Evidence> requiredEvidence;
    [SerializeField] NPCController murderer;

    [SerializeField] List<NPCController> suspectList;

    [SerializeField] UnityEvent OnSuccessfulAccusation;

    public IEnumerator Interact()
    {
        yield return DialogManager.i.ShowLine("Who would you like to accuse?");
        AccusationState.i.SetSuspects(suspectList);
        yield return GameController.i.StateMachine.PushAndWait(AccusationState.i);

        if (!AccusationState.i.HasAccused)
            yield break;

        bool successfulAccusation = true;

        if (AccusationState.i.AccusedNPC != murderer)
            successfulAccusation = false;

        var chosenEvidence = InventoryState.i.SelectedAccusationEvidence.Select(o => o.Evidence).ToList();

        foreach(var evidence in requiredEvidence)
        {
            if (!chosenEvidence.Contains(evidence))
                successfulAccusation = false;
        }

        if(successfulAccusation)
            OnSuccessfulAccusation?.Invoke();
        else
            yield return DialogManager.i.ShowLine("I don't think I'm convinced.");
    }
}
