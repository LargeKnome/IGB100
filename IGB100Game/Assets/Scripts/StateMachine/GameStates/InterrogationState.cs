using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationState : State<GameController>
{
    [SerializeField] InterrogationUI interrogationUI;

    NPCController currentInterrogatee;

    public static InterrogationState i;
    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        interrogationUI.gameObject.SetActive(true);
        interrogationUI.Init(currentInterrogatee);
    }

    public override void Execute()
    {
        interrogationUI.HandleUpdate();
    }

    public override void Exit()
    {
        interrogationUI.gameObject.SetActive(false);
    }

    public void SetCharacter(NPCController character)
    {
        currentInterrogatee = character;
    }
}
