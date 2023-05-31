using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionState : State<GameController>
{
    [SerializeField] InspectionUI inspectionUI;

    public static InspectionState i;

    public Evidence TempEvidence { get; set; }

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        inspectionUI.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (TempEvidence != null)
        {
            inspectionUI.Init(TempEvidence);
            TempEvidence = null;
        }
        else
            inspectionUI.Init(InventoryState.i.SelectedEvidence);
    }

    public override void Exit()
    {
        inspectionUI.gameObject.SetActive(false);

        if(GameController.i.StateMachine.PrevState == FreeRoamState.i)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
