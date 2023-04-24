using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycodeState : State<GameController>
{
    [SerializeField] KeycodeUI keycodeUI;

    public int CurrentCode => keycodeUI.CurrentCode;

    public static KeycodeState i;
    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        keycodeUI.gameObject.SetActive(true);

        keycodeUI.Init();
    }

    public override void Execute()
    {
        keycodeUI.HandleUpdate();
    }

    public override void Exit()
    {
        keycodeUI.gameObject.SetActive(false);
    }
}
