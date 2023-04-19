using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;

    bool typingDialog;
    Statement currentStatement;

    bool endLine;

    public static DialogManager i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public IEnumerator ShowDialog(List<Statement> statements)
    {
        typingDialog = true;

        GameController.i.StateMachine.Push(DialogState.i);

        foreach (var line in statements)
        {
            yield return TypeStatement(line);

            if (currentStatement.DisprovingEvidence == null)
                continue;

            yield return GameController.i.StateMachine.PushAndWait(ChoiceState.i);

            if (!ChoiceState.i.Refute)
                continue;

            yield return GameController.i.StateMachine.PushAndWait(InventoryState.i);

            if (!InventoryState.i.HasSelectedEvidence)
                continue;

            textMesh.color = Color.black;

            yield return TypeLine(currentStatement.StatementOnEvidence(InventoryState.i.SelectedEvidence));

            break;
        }

        GameController.i.StateMachine.Pop();
    }

    public IEnumerator ShowLine(string line)
    {
        GameController.i.StateMachine.Push(DialogState.i);

        textMesh.color = Color.black;

        yield return TypeLine(line);

        GameController.i.StateMachine.Pop();
    }

    IEnumerator TypeStatement(Statement statement)
    {
        currentStatement = statement;

        textMesh.color = (statement.DisprovingEvidence != null) ? Color.yellow : Color.black;

        yield return TypeLine(statement.Dialog);
    }

    IEnumerator TypeLine(string line)
    {
        typingDialog = true;

        string shownText = "";

        foreach (var character in line)
        {
            shownText += character;
            textMesh.text = shownText;
            yield return new WaitForSeconds(0.05f);

            if (endLine)
            {
                endLine = false;
                textMesh.text = line;
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
        if(disprovingEvidence == evidence)
            return onCorrectEvidence;
        else
            return onWrongEvidence;
    }
}
