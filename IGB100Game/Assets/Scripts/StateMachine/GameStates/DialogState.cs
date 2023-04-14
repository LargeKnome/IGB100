using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogState : State<GameController>
{
    [SerializeField] GameObject dialogBox;

    public static DialogState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        dialogBox.SetActive(true);
    }

    public override void Execute()
    {
        DialogManager.i.HandleUpdate();
    }

    public override void Exit()
    {
        dialogBox.SetActive(false);
    }
}
