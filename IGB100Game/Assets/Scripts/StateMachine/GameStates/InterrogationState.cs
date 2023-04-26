using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationState : State<GameController>
{
    [SerializeField] InterrogationUI interrogationUI;
    [SerializeField] QuestionUI questionUI;

    NPCController currentSuspect;
    public NPCController CurrentSuspect => currentSuspect;

    public static InterrogationState i;
    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        interrogationUI.gameObject.SetActive(true);
        interrogationUI.Init(currentSuspect);

        interrogationUI.OnSelect += HandleSelection;
    }

    public override void Execute()
    {
        interrogationUI.HandleUpdate();
    }

    public override void Exit()
    {
        interrogationUI.OnSelect -= HandleSelection;
        interrogationUI.gameObject.SetActive(false);
    }

    void HandleSelection(int selection)
    {
        StartCoroutine(interrogationUI.HandleSelectionAsync(selection));
    }

    public void SetCharacter(NPCController character)
    {
        currentSuspect = character;
    }

    public void AskQuestion(Question question)
    {
        interrogationUI.AskQuestion(question);
    }
}
