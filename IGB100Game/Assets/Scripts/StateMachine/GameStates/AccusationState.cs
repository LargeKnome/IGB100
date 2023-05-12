using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccusationState : State<GameController>
{
    [SerializeField] AccusationUI accusationUI;

    public bool HasAccused => accusationUI.HasAccused;

    public static AccusationState i;

    public Evidence Suspect => accusationUI.SelectedSuspect;
    public Evidence Weapon => accusationUI.SelectedWeapon;
    public Evidence Motive => accusationUI.SelectedMotive;

    GameController gc;

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        gc = owner;
        accusationUI.gameObject.SetActive(true);
        accusationUI.Init();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void Exit()
    {
        accusationUI.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
