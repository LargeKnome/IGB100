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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        interrogationUI.gameObject.SetActive(true);
        interrogationUI.Init(CurrentSuspect);
    }

    public override void Execute()
    {
        interrogationUI.HandleScrolling();
    }

    public override void Exit()
    {
        interrogationUI.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
