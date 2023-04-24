using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;

    Statement currentStatement;

    public static DialogManager i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public IEnumerator ShowDialog(List<Statement> statements)
    {
        GameController.i.StateMachine.Push(DialogState.i);

        foreach (var line in statements)
        {
            yield return TypeStatement(line);

            yield return GameController.i.StateMachine.PushAndWait(InventoryState.i);

            if (!InventoryState.i.HasSelectedEvidence)
                continue;

            yield return TypeLine(currentStatement.StatementOnEvidence(InventoryState.i.SelectedEvidence));

            break;
        }

        GameController.i.StateMachine.Pop();
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

    IEnumerator TypeStatement(Statement statement)
    {
        currentStatement = statement;

        yield return TypeLine(statement.Dialog);
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
    [SerializeField] string question;
    [SerializeField] Statement response;
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
