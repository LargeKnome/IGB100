using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycodeState : State<GameController>
{
    [SerializeField] KeycodeUI keycodeUI;

    public int CurrentCode => keycodeUI.CurrentCode;
    public bool Submitted => keycodeUI.Submitted;

    public static KeycodeState i;
    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        keycodeUI.gameObject.SetActive(true);

        keycodeUI.Init();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void Exit()
    {
        keycodeUI.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
