using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionState : State<GameController>
{
    [SerializeField] InspectionUI inspectionUI;

    public static InspectionState i;

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        inspectionUI.gameObject.SetActive(true);
        inspectionUI.Init(InventoryState.i.SelectedEvidence);
    }

    public override void Exit()
    {
        inspectionUI.gameObject.SetActive(false);
    }
}
