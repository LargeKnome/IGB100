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

    public IEnumerator ShowChoiceLine(string line)
    {
        GameController.i.StateMachine.Push(DialogState.i);

        yield return TypeLine(line);

        yield return GameController.i.StateMachine.PushAndWait(ChoiceState.i);

        GameController.i.StateMachine.Pop();
    }

    IEnumerator TypeLine(string line)
    {
        textMesh.text = line;

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
    }
}

[Serializable]
public class Question
{
    [SerializeField] string questionText;
    [SerializeField] Statement response;

    public string QuestionText => questionText;
    public Statement Response => response;
}

[Serializable]
public class Statement
{
    [SerializeField] string statement;
    [SerializeField] Evidence disprovingEvidence;
    [SerializeField] string onCorrectEvidence;
    [SerializeField] string onWrongEvidence;

    public string Dialog => statement;
    public Evidence DisprovingEvidence => disprovingEvidence;

    public string StatementOnEvidence(Evidence evidence)
    {
        if (disprovingEvidence == null)
            return onWrongEvidence;
        if(disprovingEvidence == evidence)
            return onCorrectEvidence;
        else
            return onWrongEvidence;
    }
}
