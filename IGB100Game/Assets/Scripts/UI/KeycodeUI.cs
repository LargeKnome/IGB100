using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class KeycodeUI : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> keys;

    bool changeHorizontal;
    bool changeVertical;

    int selectedKey;

    public int CurrentCode { get; private set; }

    public void Init()
    {
        selectedKey = 0;
        keys[selectedKey].color = Color.yellow;
        UpdateCode();
    }

    public void HandleUpdate()
    {
        foreach (TextMeshProUGUI key in keys)
            key.color = Color.white;

        var input = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));

        if (changeHorizontal && input != 0)
        {
            selectedKey += input;
            changeHorizontal = false;
        }
        else if (input == 0)
            changeHorizontal = true;

        selectedKey = Mathf.Clamp(selectedKey, 0, keys.Count - 1);
        keys[selectedKey].color = Color.yellow;


        HandleKeyUpdate(keys[selectedKey]);

        if (Input.GetButtonDown("Submit"))
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

        if (currentValue < 0)
            currentValue = 9;
        if (currentValue > 9)
            currentValue = 0;

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

        Debug.Log(currentCode);
        CurrentCode = Convert.ToInt16(currentCode);
    }
}
