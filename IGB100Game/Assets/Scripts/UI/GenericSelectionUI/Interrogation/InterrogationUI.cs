using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InterrogationUI : SelectionUI<InterrogationButtonUI>
{
    [Header("Character Info")]
    [SerializeField] Image characterProfile;
    [SerializeField] TextMeshProUGUI characterName;

    [Header("UI Objects")]
    [SerializeField] GameObject dialogParent;
    [SerializeField] List<InterrogationButtonUI> buttons;
    [SerializeField] GameObject dialogTextPrefab;

    List<Statement> npcStatements;
    NPCController currentSuspect;
    Statement currentStatement;

    public void Init(NPCController suspect)
    {
        currentStatement = null;
        currentSuspect = suspect;
        characterName.text = suspect.name;

        npcStatements = new();

        foreach (Transform textObj in dialogParent.transform)
            Destroy(textObj.gameObject);

        foreach(var answeredQuestion in suspect.AnsweredQuestions)
            AskQuestion(answeredQuestion);

        SetItems(buttons, 1);
    }

    public void AskQuestion(Question question)
    {
        AddText(question.QuestionText, false);
        AddStatement(question.Response);

        if(!currentSuspect.AnsweredQuestions.Contains(question))
            currentSuspect.AnsweredQuestions.Add(question);
    }

    void AddText(string text, bool fromNPC)
    {
        var newDialog = Instantiate(dialogTextPrefab, dialogParent.transform);

        newDialog.GetComponent<TextMeshProUGUI>().text = text;
        newDialog.GetComponent<TextMeshProUGUI>().color = (fromNPC) ? Color.black : Color.magenta;
    }

    void AddStatement(Statement statement)
    {
        currentStatement = statement;
        npcStatements.Add(statement);

        var newDialog = Instantiate(dialogTextPrefab, dialogParent.transform);

        newDialog.GetComponent<TextMeshProUGUI>().text = statement.Dialog;
        newDialog.GetComponent<TextMeshProUGUI>().color = Color.black;
    }

    public IEnumerator HandleSelectionAsync(int selection)
    {
        if (selection == 0) // Question
        {
            if (currentSuspect.AnsweredQuestions.Count == currentSuspect.InterrogationQuestions.Count)
                yield break;

            yield return GameController.i.StateMachine.PushAndWait(QuestionState.i);
        }
        else if (selection == 1) //Refute
        {
            if (currentStatement == null)
                yield break;

            yield return GameController.i.StateMachine.PushAndWait(InventoryState.i);

            if (InventoryState.i.HasSelectedEvidence)
            {
                AddText(InventoryState.i.SelectedEvidence.Name + " was shown.", false);
                AddText(currentStatement.StatementOnEvidence(InventoryState.i.SelectedEvidence), true);
            }
        }
        else if (selection == 2) //Exit
            GameController.i.StateMachine.Pop();

        yield return null;
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

    public string StatementOnEvidence(Evidence evidence)
    {
        if (disprovingEvidence == null)
            return onWrongEvidence;
        if (disprovingEvidence == evidence)
            return onCorrectEvidence;
        else
            return onWrongEvidence;
    }
}
