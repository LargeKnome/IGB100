using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : State<GameController>
{
    [SerializeField] InventoryUI inventoryUI;

    public static InventoryState i;

    public Evidence SelectedEvidence => inventoryUI.SelectedEvidence;

    public bool HasSelectedEvidence { get; private set; }

    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        gc = owner;
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.Init();

        HasSelectedEvidence = false;
    }

    public override void Execute()
    {
        inventoryUI.HandleUpdate();

        if (inventoryUI.CurrentInventory.Count == 0) return;

        if (gc.StateMachine.PrevState == DialogState.i)
        {
            if (Input.GetButtonDown("Interact"))
            {
                HasSelectedEvidence = true;
                gc.StateMachine.Pop();
            }
        }
    }

    public override void Exit()
    {
        inventoryUI.gameObject.SetActive(false);
    }
}
