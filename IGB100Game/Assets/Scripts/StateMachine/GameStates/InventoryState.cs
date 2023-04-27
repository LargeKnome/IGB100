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

        inventoryUI.OnSelect += OnSelect;
        inventoryUI.OnExit += OnBack;

        HasSelectedEvidence = false;
    }

    public override void Execute()
    {
        inventoryUI.HandleUpdate();
    }

    public override void Exit()
    {
        inventoryUI.OnSelect -= OnSelect;
        inventoryUI.OnExit -= OnBack;

        inventoryUI.gameObject.SetActive(false);
    }

    void OnSelect(int selected)
    {
        if (gc.StateMachine.PrevState == RefuteState.i)
        {
            HasSelectedEvidence = true;
            gc.StateMachine.Pop();
        }
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
