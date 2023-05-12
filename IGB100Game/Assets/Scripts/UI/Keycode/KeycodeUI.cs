using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class KeycodeUI : MonoBehaviour
{
    [SerializeField] List<KeycodeNumUI> keys;

    public int CurrentCode { get; private set; }
    public bool Submitted { get; private set; }

    public void Init()
    {
        Submitted = false;

        foreach (var key in keys)
        {
            key.Init();
            key.onChanged += UpdateCode;
        }

        UpdateCode();
    }

    public void UpdateCode()
    {
        CurrentCode = keys[0].CurrentNum * 1000 + keys[1].CurrentNum * 100 + keys[2].CurrentNum * 10 + keys[3].CurrentNum;
    }

    public void OnSubmit()
    {
        Submitted = true;
        GameController.i.StateMachine.Pop();
    }
}
