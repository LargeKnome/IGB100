using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject evidencePrefab;
    [SerializeField] RectTransform evidenceParent;

    [SerializeField] TextMeshProUGUI evidenceName;
    [SerializeField] TextMeshProUGUI evidenceDescription;

    List<EvidenceUI> currentInventory;
    public List<EvidenceUI> CurrentInventory => currentInventory;

    const int gridColumnCount = 4;

    public List<EvidenceUI> SelectedAccusationEvidence { get; private set; }
    public Evidence SelectedEvidence { get; private set; }
    public bool HasSelectedEvidence { get; private set; }

    public void Init()
    {
        HasSelectedEvidence = false;

        foreach(Transform child in evidenceParent)
            Destroy(child.gameObject);

        currentInventory = new List<EvidenceUI>();

        foreach(var evidence in GameController.i.Player.Inventory.EvidenceList)
        {
            var evidenceObj = Instantiate(evidencePrefab);
            var evidenceUI = evidenceObj.GetComponent<EvidenceUI>();
            evidenceUI.Init(evidence);
            evidenceUI.onHoverEnter += OnHoverChanged;

            evidenceObj.transform.SetParent(evidenceParent, false);
            currentInventory.Add(evidenceUI);
            evidenceObj.GetComponent<Button>().onClick.AddListener(delegate { OnSelect(evidenceUI); });
        }

        if (currentInventory.Count == 0)
        {
            evidenceDescription.text = "No evidence found.";
            evidenceName.gameObject.SetActive(false);
            return;
        }
        else
        {
            evidenceName.gameObject.SetActive(false);
            evidenceDescription.gameObject.SetActive(false);
        }
    }

    void OnSelect(EvidenceUI selectedUI)
    {
        var prevState = GameController.i.StateMachine.PrevState;

        if (prevState == InterrogationState.i)
        {
            SelectedEvidence = selectedUI.Evidence;
            HasSelectedEvidence = true;
            GameController.i.StateMachine.Pop();
        }
        else if (prevState == AccusationState.i)
        {
            if (SelectedAccusationEvidence.Contains(selectedUI))
            {
                SelectedAccusationEvidence.Remove(selectedUI);
                selectedUI.SetSelected(false);
            }
            else
            {
                SelectedAccusationEvidence.Add(selectedUI);
                selectedUI.SetSelected(true);
            }

            HasSelectedEvidence = SelectedAccusationEvidence.Count > 0;
        }
    }

    public void OnHoverChanged(EvidenceUI currentHover)
    {
        evidenceName.gameObject.SetActive(true);
        evidenceDescription.gameObject.SetActive(true);

        evidenceName.text = currentHover.Evidence.Name;
        evidenceDescription.text = "";

        foreach (string line in currentHover.Evidence.Description)
            evidenceDescription.text += line+" ";
    }
}
