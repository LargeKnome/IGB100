using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationState : State<GameController>
{
    [SerializeField] InterrogationUI interrogationUI;

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        interrogationUI.gameObject.SetActive(false);
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
