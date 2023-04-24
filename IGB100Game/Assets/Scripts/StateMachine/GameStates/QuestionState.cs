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
        questionUI.Init(InterrogationState.i.CurrentInterrogatee.InterrogationQuestions);
    }

    public override void Execute()
    {
        questionUI.HandleUpdate();
    }

    public override void Exit()
    {
        questionUI.gameObject.SetActive(false);
    }
}
