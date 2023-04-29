using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccusationState : State<GameController>
{
    [SerializeField] NPCSelectionUI npcSelectionUI;

    public NPCController AccusedNPC => npcSelectionUI.GetItemAtSelection().CurrentNPC;

    public bool HasAccused { get; private set; }

    List<NPCController> suspects;

    public static AccusationState i;

    GameController gc;

    private void Awake()
    {
        i = this;
    }

    public override void Enter(GameController owner)
    {
        HasAccused = false;

        gc = owner;
        npcSelectionUI.Init(suspects);
        npcSelectionUI.gameObject.SetActive(true);

        npcSelectionUI.OnSelect += OnSelect;
        npcSelectionUI.OnExit += OnExit;
    }

    public override void Execute()
    {
        npcSelectionUI.HandleUpdate();
    }

    public override void Exit()
    {
        npcSelectionUI.OnSelect -= OnSelect;
        npcSelectionUI.OnExit -= OnExit;
        npcSelectionUI.gameObject.SetActive(false);
    }

    public void OnSelect(int selection)
    {
        StartCoroutine(HandleSelect());
    }

    IEnumerator HandleSelect()
    {
        yield return gc.StateMachine.PushAndWait(InventoryState.i);

        if(InventoryState.i.HasSelectedEvidence)
        {
            HasAccused = true;
            gc.StateMachine.Pop();
        }
    }

    public void OnExit()
    {
        gc.StateMachine.Pop();
    }

    public void SetSuspects(List<NPCController> suspectList)
    {
        suspects = suspectList;
    }
}
