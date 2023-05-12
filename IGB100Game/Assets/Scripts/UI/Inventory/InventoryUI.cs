using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Evidence List")]
    [SerializeField] GameObject evidencePrefab;
    [SerializeField] RectTransform evidenceParent;
    [SerializeField] TextMeshProUGUI categoryTitle;

    [Header("Evidence Info")]
    [SerializeField] TextMeshProUGUI evidenceName;
    [SerializeField] TextMeshProUGUI evidenceDescription;

    List<EvidenceUI> currentInventory;
    public List<EvidenceUI> CurrentInventory => currentInventory;

    const int gridColumnCount = 4;

    public List<EvidenceUI> SelectedAccusationEvidence { get; private set; }
    public Evidence SelectedEvidence { get; private set; }
    public bool HasSelectedEvidence { get; private set; }

    int selectedCategory;

    string[] categoryNames = new string[] {"Objects", "Keys", "Statements"};

    public void Init()
    {
        HasSelectedEvidence = false;

        foreach(Transform child in evidenceParent)
            Destroy(child.gameObject);

        currentInventory = new List<EvidenceUI>();

        foreach(var evidence in GameController.i.Player.Inventory.Evidence[selectedCategory])
        {
            var evidenceObj = Instantiate(evidencePrefab);
            var evidenceUI = evidenceObj.GetComponent<EvidenceUI>();
            evidenceUI.Init(evidence);
            evidenceUI.onHoverEnter += OnHoverChanged;

            evidenceObj.transform.SetParent(evidenceParent, false);
            currentInventory.Add(evidenceUI);
            evidenceObj.GetComponent<Button>().onClick.AddListener(delegate { OnSelect(evidenceUI); });
        }

        categoryTitle.text = categoryNames[selectedCategory];

        evidenceName.gameObject.SetActive(false);

        if (currentInventory.Count == 0)
        {
            evidenceDescription.gameObject.SetActive(true);
            evidenceDescription.text = "No evidence found.";
        }
        else
            evidenceDescription.gameObject.SetActive(false);
    }

    public void ChangeCategory(int diff)
    {
        selectedCategory += diff;

        if (selectedCategory > GameController.i.Player.Inventory.Evidence.Count - 1)
            selectedCategory = 0;
        else if (selectedCategory < 0)
            selectedCategory = GameController.i.Player.Inventory.Evidence.Count - 1;

        Init();
    }

    void OnSelect(EvidenceUI selectedUI)
    {
        SelectedEvidence = selectedUI.Evidence;
        HasSelectedEvidence = true;
        GameController.i.StateMachine.Pop();
    }

    public void OnHoverChanged(EvidenceUI currentHover)
    {
        evidenceName.gameObject.SetActive(true);
        evidenceDescription.gameObject.SetActive(true);

        evidenceName.text = currentHover.Evidence.Name;
        evidenceDescription.text = "";

        foreach (string line in currentHover.Evidence.Description)
            evidenceDescription.text += line + "\n";
    }
}
