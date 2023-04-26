using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationState : State<GameController>
{
    [SerializeField] InterrogationUI interrogationUI;
    [SerializeField] QuestionUI questionUI;

    public NPCController CurrentSuspect { get; private set; }

    public static InterrogationState i;
    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        interrogationUI.gameObject.SetActive(true);
        interrogationUI.Init(CurrentSuspect);

        interrogationUI.OnSelect += OnSelected;
    }

    public override void Execute()
    {
        interrogationUI.HandleUpdate();
    }

    public override void Exit()
    {
        interrogationUI.OnSelect -= OnSelected;
        interrogationUI.gameObject.SetActive(false);
    }

    void OnSelected(int selection)
    {
        StartCoroutine(interrogationUI.HandleSelection(selection));
    }

    public void SetSuspect(NPCController character)
    {
        CurrentSuspect = character;
    }

    public void AskQuestion(Question question)
    {
        interrogationUI.AskQuestion(question);
    }
}
