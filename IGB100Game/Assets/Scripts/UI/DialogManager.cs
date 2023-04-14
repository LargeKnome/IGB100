using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;

    bool typingDialog;
    string currentLine;
    bool endLine;

    public static DialogManager i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        typingDialog = true;

        GameController.i.StateMachine.Push(DialogState.i);

        foreach(var line in dialog.Lines)
        {
            yield return TypeLine(line);
        }

        GameController.i.StateMachine.Pop();
    }

    IEnumerator TypeLine(string line)
    {
        typingDialog = true;
        currentLine = line;

        string shownText = "";

        foreach (var character in line)
        {
            shownText += character;
            textMesh.text = shownText;
            yield return new WaitForSeconds(0.05f);

            if (endLine)
            {
                endLine = false;
                textMesh.text = currentLine;
                typingDialog = false;
                break;
            }
        }

        typingDialog = false;

        yield return new WaitForEndOfFrame();

        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
    }

    public void HandleUpdate()
    {
        if(typingDialog && Input.GetButtonDown("Interact"))
            endLine = true;
    }
}

[Serializable]
public class Dialog
{
    [SerializeField] List<string> lines;

    public Dialog(string start)
    {
        lines = new List<string>
        {
            start
        };
    }

    public void AddDialog(string newLine)
    {
        lines.Add(newLine);
    }

    public List<string> Lines => lines;
}
