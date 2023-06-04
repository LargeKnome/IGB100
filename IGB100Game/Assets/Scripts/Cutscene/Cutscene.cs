using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeReference]
    [SerializeField] List<CutsceneAction> actions;

    IEnumerator PlayCutscene()
    {
        GameController.i.StateMachine.Push(CutsceneState.i);

        foreach (var action in actions)
            yield return action.Play();

        GameController.i.StateMachine.Pop();
    }

    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    public void AddAction(CutsceneAction action)
    {
        actions.Add(action);
    }
}
