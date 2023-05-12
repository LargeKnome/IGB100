using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccusationUI : MonoBehaviour
{
    [SerializeField] EvidenceUI suspectBox;
    [SerializeField] EvidenceUI weaponBox;
    [SerializeField] EvidenceUI motiveBox;

    public Evidence SelectedSuspect => suspectBox.Evidence;
    public Evidence SelectedWeapon => weaponBox.Evidence;
    public Evidence SelectedMotive => motiveBox.Evidence;

    public bool HasAccused { get; private set; }

    EvidenceUI selectedBox;

    public void Init()
    {
        HasAccused = false;

        suspectBox.Init(null);
        weaponBox.Init(null);
        motiveBox.Init(null);
    }

    public void OnSuspectSelect()
    {
        selectedBox = suspectBox;
        StartCoroutine(PushInventory());
    }

    public void OnWeaponSelect()
    {
        selectedBox = weaponBox;
        StartCoroutine(PushInventory());
    }

    public void OnMotiveSelect()
    {
        selectedBox = motiveBox;
        StartCoroutine(PushInventory());
    }

    IEnumerator PushInventory()
    {
        yield return GameController.i.StateMachine.PushAndWait(InventoryState.i);

        if (InventoryState.i.HasSelectedEvidence)
            selectedBox.Init(InventoryState.i.SelectedEvidence);
    }

    public void OnSubmit()
    {
        HasAccused = true;
        GameController.i.StateMachine.Pop();
    }
}
