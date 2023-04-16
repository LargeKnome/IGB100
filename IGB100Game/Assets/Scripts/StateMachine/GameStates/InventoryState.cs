using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : State<GameController>
{
    [SerializeField] InventoryUI inventoryUI;

    public static InventoryState i;

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.Init();
    }

    public override void Execute()
    {
        inventoryUI.HandleUpdate();
    }

    public override void Exit()
    {
        inventoryUI.gameObject.SetActive(false);
    }
}
