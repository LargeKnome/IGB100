using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionUI<T> : MonoBehaviour where T : ISelectableItem
{
    public Action<int> OnSelect;
    public Action OnExit;

    List<T> items;

    int currentSelection;
    int prevSelection;
    int selectionWidth;
    bool changeSelection;

    public void SetItems(List<T> items, int gridWidth)
    {
        currentSelection = 0;
        this.items = items;
        selectionWidth = gridWidth;

        if (items == null) return;
        if (items.Count == 0) return;

        foreach (var item in items)
            item.OnSelectionChanged(false);

        OnSelectionChanged(true);
    }

    public virtual void HandleUpdate()
    {
        if (Input.GetButtonDown("Back"))
            OnExit?.Invoke();

        if (items == null) return;
        if (items.Count == 0) return;

        prevSelection = currentSelection;

        if (selectionWidth > 1)
            HandleGridSelection();
        else
            HandleListselection();

        currentSelection = Mathf.Clamp(currentSelection, 0, items.Count - 1);

        if (currentSelection != prevSelection)
            OnSelectionChanged(false);

        if (Input.GetButtonDown("Interact"))
            OnSelect?.Invoke(currentSelection);
    }
    
    void HandleListselection()
    {
        var input = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (input != 0 && changeSelection)
        {
            currentSelection -= input;
            changeSelection = false;
        }
        else if (input == 0)
            changeSelection = true;
    }

    void HandleGridSelection()
    {
        int horizontalInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        int verticalInput = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (changeSelection)
        {
            if (horizontalInput != 0)
            {
                currentSelection += horizontalInput;
                changeSelection = false;
            }
            else if (verticalInput != 0)
            {
                currentSelection -= verticalInput * selectionWidth;
                changeSelection = false;
            }
        }
        else if (horizontalInput == 0 && verticalInput == 0)
            changeSelection = true;
    }

    public virtual void OnSelectionChanged(bool onInit)
    {
        items[currentSelection].OnSelectionChanged(true);

        if(!onInit)
            items[prevSelection].OnSelectionChanged(false);
    }

    public T GetItemAtSelection()
    {
        return items[currentSelection];
    }
}
