using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : State<GameController>
{
    [SerializeField] InventoryUI inventoryUI;

    public static InventoryState i;

    public Evidence SelectedEvidence => inventoryUI.SelectedEvidence;

    public List<EvidenceUI> SelectedAccusationEvidence => inventoryUI.SelectedAccusationEvidence;

    public bool HasSelectedEvidence => inventoryUI.HasSelectedEvidence;

    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gc = owner;
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.Init();
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && gc.StateMachine.PrevState == FreeRoamState.i)
            gc.StateMachine.Pop();

        if (Input.GetButtonDown("Exit") && gc.StateMachine.PrevState != FreeRoamState.i)
            gc.StateMachine.Pop();
    }

    public override void Exit()
    {
        if(gc.StateMachine.PrevState == FreeRoamState.i)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        inventoryUI.gameObject.SetActive(false);
    }
}
