using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class KeycodeUI : MonoBehaviour
{
    List<TextMeshProUGUI> keys;

    bool changeHorizontal;
    bool changeVertical;

    int selectedKey;

    public int CurrentCode { get; private set; }

    public void Init()
    {
        keys = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        selectedKey = 0;
        keys[selectedKey].color = Color.yellow;
        UpdateCode();
    }

    public void HandleUpdate()
    {
        int prevKey = selectedKey;

        var input = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));

        if (changeHorizontal && input != 0)
        {
            selectedKey += input;
            changeHorizontal = false;
        }
        else if (input == 0)
            changeHorizontal = true;

        selectedKey = Mathf.Clamp(selectedKey, 0, keys.Count - 1);

        if(prevKey != selectedKey)
        {
            keys[prevKey].color = Color.white;
            keys[selectedKey].color = Color.yellow;
        }

        HandleKeyUpdate(keys[selectedKey]);

        if (Input.GetButtonDown("Interact") || Input.GetButtonDown("Back"))
            GameController.i.StateMachine.Pop();
    }

    void HandleKeyUpdate(TextMeshProUGUI key)
    {
        int currentValue = Convert.ToInt16(key.text);
        int prevValue = currentValue;

        var input = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (changeVertical && input != 0)
        {
            currentValue += input;
            changeVertical = false;
        }
        else if (input == 0)
            changeVertical = true;

        if (currentValue < 1)
            currentValue = 9;
        if (currentValue > 9)
            currentValue = 1;

        if (prevValue != currentValue)
        {
            key.text = currentValue.ToString();

            UpdateCode();
        }
    }

    void UpdateCode()
    {
        string currentCode = "";

        foreach (var keyCode in keys)
            currentCode += keyCode.text;

        CurrentCode = Convert.ToInt16(currentCode);
    }
}
