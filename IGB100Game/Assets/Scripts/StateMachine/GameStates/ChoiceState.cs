using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceState : State<GameController>
{
    [SerializeField] ChoiceBox choiceBox;


    public static ChoiceState i;

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        choiceBox.gameObject.SetActive(true);
    }

    public override void Execute()
    {
        choiceBox.HandleUpdate();
    }

    public override void Exit()
    {
        choiceBox.gameObject.SetActive(false);
    }
}
