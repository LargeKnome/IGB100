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

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;

        questionUI.gameObject.SetActive(true);
        questionUI.Init(InterrogationState.i.CurrentSuspect);
    }

    public override void Exit()
    {
        questionUI.gameObject.SetActive(false);
    }    
}
