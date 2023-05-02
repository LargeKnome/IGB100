using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : State<GameController>
{
    [SerializeField] InventoryUI inventoryUI;

    public static InventoryState i;

    public Evidence SelectedEvidence => inventoryUI.SelectedEvidence;

    public List<EvidenceUI> SelectedAccusationEvidence { get; private set; }

    public bool HasSelectedEvidence { get; private set; }

    private void Awake()
    {
        i = this;
    }

    GameController gc;

    public override void Enter(GameController owner)
    {
        SelectedAccusationEvidence = new();

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

        if (gc.StateMachine.PrevState == AccusationState.i)
        {
            foreach (var evidenceUI in SelectedAccusationEvidence)
                evidenceUI.OnSelectionChanged(true);
        }
    }

    public override void Exit()
    {
        inventoryUI.OnSelect -= OnSelect;
        inventoryUI.OnExit -= OnBack;

        inventoryUI.gameObject.SetActive(false);
    }

    void OnSelect(int selected)
    {
        if (gc.StateMachine.PrevState == InterrogationState.i)
        {
            HasSelectedEvidence = true;
            gc.StateMachine.Pop();
        }
        else if(gc.StateMachine.PrevState == AccusationState.i)
        {
            EvidenceUI currentSelection = inventoryUI.GetItemAtSelection();

            if (SelectedAccusationEvidence.Contains(currentSelection))
                SelectedAccusationEvidence.Remove(currentSelection);
            else
                SelectedAccusationEvidence.Add(currentSelection);

            HasSelectedEvidence = SelectedAccusationEvidence.Count > 0;
        }
    }

    void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
