using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuteState : State<GameController>
{
    [SerializeField] RefuteUI refuteUI;

    public static RefuteState i;

    List<InterrogationTextUI> interrogationTexts;

    public bool HasRefutedStatement { get; private set; }

    public Statement SelectedStatement => refuteUI.GetItemAtSelection().CurrentStatement;

    private void Awake()
    {
        i = this;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        HasRefutedStatement = false;

        refuteUI.OnSelect += OnSelection;
        refuteUI.OnExit += OnExit;

        refuteUI.Init(interrogationTexts);

        refuteUI.SetSelection(interrogationTexts.Count - 1);
    }

    public override void Execute()
    {
        refuteUI.HandleUpdate();
    }

    public override void Exit()
    {
        refuteUI.OnSelect -= OnSelection;
        refuteUI.OnExit -= OnExit;

        refuteUI.ResetScrolling();

        refuteUI.GetItemAtSelection().OnSelectionChanged(false);
    }

    void OnSelection(int selection)
    {
        StartCoroutine(OnSelectionAsync());
    }

    IEnumerator OnSelectionAsync()
    {
        if (refuteUI.GetItemAtSelection() == null)
            yield break;

        if(refuteUI.GetItemAtSelection().CurrentStatement == null)
            yield break;

        yield return gc.StateMachine.PushAndWait(InventoryState.i);

        if (InventoryState.i.HasSelectedEvidence)
        {
            HasRefutedStatement = true;
            refuteUI.ResetScrolling();
            gc.StateMachine.Pop();
        }
    }

    void OnExit()
    {
        gc.StateMachine.Pop();
    }

    public void SetInterrogationTexts(List<InterrogationTextUI> interrogationTexts)
    {
        this.interrogationTexts = interrogationTexts;
    }
}
