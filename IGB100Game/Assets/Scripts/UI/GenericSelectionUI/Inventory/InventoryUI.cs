using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : SelectionUI<EvidenceUI>
{
    [SerializeField] GameObject evidencePrefab;
    [SerializeField] RectTransform evidenceParent;

    [SerializeField] TextMeshProUGUI evidenceName;
    [SerializeField] TextMeshProUGUI evidenceDescription;

    List<EvidenceUI> currentInventory;
    public List<EvidenceUI> CurrentInventory => currentInventory;

    const int gridColumnCount = 4;

    public Evidence SelectedEvidence => (currentInventory.Count == 0) ? null : GetItemAtSelection().Evidence;

    public void Init()
    {
        foreach(Transform child in evidenceParent)
            Destroy(child.gameObject);

        currentInventory = new List<EvidenceUI>();

        foreach(var evidence in GameController.i.Player.Inventory.EvidenceList)
        {
            var evidenceObj = Instantiate(evidencePrefab);
            evidenceObj.GetComponent<EvidenceUI>().Init(evidence);
            evidenceObj.transform.SetParent(evidenceParent, false);
            currentInventory.Add(evidenceObj.GetComponent<EvidenceUI>());
        }

        if (currentInventory.Count == 0)
        {
            evidenceDescription.text = "No evidence found.";
            evidenceName.gameObject.SetActive(false);
            return;
        }

        SetItems(currentInventory, gridColumnCount);
        evidenceName.gameObject.SetActive(true);
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

        if (currentInventory.Count == 0)
            return;

        GetItemAtSelection().HandleUpdate();
    }

    public override void OnSelectionChanged(bool onInit)
    {
        base.OnSelectionChanged(onInit);

        evidenceName.text = SelectedEvidence.Name;
        evidenceDescription.text = "";

        foreach (string line in SelectedEvidence.Description)
            evidenceDescription.text += line+"\n";

    }
}
