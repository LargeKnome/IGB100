using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject evidencePrefab;
    [SerializeField] RectTransform evidenceParent;

    [SerializeField] TextMeshProUGUI evidenceName;
    [SerializeField] TextMeshProUGUI evidenceDescription;

    List<EvidenceUI> currentInventory;

    int selectedIndex;
    bool changeSelection = true;

    const int gridColumnCount = 4;

    public Evidence SelectedEvidence => (currentInventory.Count == 0) ? null : currentInventory[selectedIndex].Evidence;

    public void Init()
    {
        selectedIndex = 0;

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

        UpdateSelection();

        if (currentInventory.Count != 0)
            currentInventory[selectedIndex].OnSelected(true);
    }

    public void HandleUpdate()
    {
        if (Input.GetButtonDown("Back"))
            GameController.i.StateMachine.Pop();

        if (currentInventory.Count == 0) return;

        currentInventory[selectedIndex].HandleUpdate();

        int prevIndex = selectedIndex;

        int horizontalInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        int verticalInput = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (changeSelection)
        {
            if (horizontalInput != 0)
            {
                selectedIndex += horizontalInput;
                changeSelection = false;
            }
            else if (verticalInput != 0)
            {
                selectedIndex -= verticalInput * gridColumnCount;
                changeSelection = false;
            }
        }
        else if (horizontalInput == 0 && verticalInput == 0)
            changeSelection = true;

        selectedIndex = Mathf.Clamp(selectedIndex, 0, currentInventory.Count - 1);

        if(selectedIndex != prevIndex)
        {
            currentInventory[selectedIndex].OnSelected(true);
            currentInventory[prevIndex].OnSelected(false);
            UpdateSelection();
        }
    }

    void UpdateSelection()
    {
        if (SelectedEvidence != null)
        {
            evidenceName.text = SelectedEvidence.Name;
            evidenceDescription.text = SelectedEvidence.Description;
        }
        else
            evidenceDescription.text = "No evidence found.";

        evidenceName.gameObject.SetActive(SelectedEvidence != null);
    }
}
