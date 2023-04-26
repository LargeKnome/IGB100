using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;

    public static DialogManager i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public IEnumerator ShowLine(string line)
    {
        GameController.i.StateMachine.Push(DialogState.i);

        yield return TypeLine(line);

        GameController.i.StateMachine.Pop();
    }

    IEnumerator TypeLine(string line)
    {
        textMesh.text = line;

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
    }
}