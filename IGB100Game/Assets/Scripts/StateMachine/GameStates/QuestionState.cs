using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionState : State<GameController>
{
    [SerializeField] QuestionUI questionUI;

    public static QuestionState i;

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        questionUI.gameObject.SetActive(true);
        questionUI.Init(InterrogationState.i.CurrentSuspect);

        questionUI.OnSelect += OnSelected;
        questionUI.OnExit += OnExit;
    }

    public override void Execute()
    {
        questionUI.HandleUpdate();
    }

    public override void Exit()
    {
        questionUI.OnSelect -= OnSelected;
        questionUI.OnExit -= OnExit;
        questionUI.gameObject.SetActive(false);
    }

    private void OnSelected(int selection)
    {
        InterrogationState.i.AskQuestion(questionUI.GetItemAtSelection().CurrentQuestion);
        GameController.i.StateMachine.Pop();
    }

    void OnExit()
    {
        GameController.i.StateMachine.Pop();
    }
    
}
