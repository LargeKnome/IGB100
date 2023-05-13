using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeycodeNumUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numText;

    public event Action onChanged;

    public int CurrentNum { get; private set; }

    public void Init()
    {
        CurrentNum = 0;
        numText.text = CurrentNum.ToString();
    }

    public void ChangeNum(int change)
    {
        CurrentNum += change;

        if (CurrentNum < 0)
            CurrentNum = 9;
        else if(CurrentNum > 9)
            CurrentNum = 0;

        numText.text = CurrentNum.ToString();

        onChanged?.Invoke();
    }
}
