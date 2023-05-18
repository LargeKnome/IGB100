using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InterrogationUI : MonoBehaviour
{
    [Header("Character Info")]
    [SerializeField] Image characterProfile;
    [SerializeField] TextMeshProUGUI characterName;

    [Header("UI Objects")]
    [SerializeField] RectTransform dialogParent;
    [SerializeField] List<GenericButton> buttons;
    [SerializeField] GameObject dialogTextPrefab;

    [Header("EvidencePrefabs")]
    [SerializeField] GameObject statementPrefab;

    [SerializeField] float scrollSpeed;

    List<InterrogationTextUI> textUIs;
    NPCController currentSuspect;

    InterrogationTextUI selectedStatement;

    public void Init(NPCController suspect)
    {
        currentSuspect = suspect;
        characterName.text = suspect.name;

        textUIs = new();

        foreach (Transform textObj in dialogParent.transform)
            Destroy(textObj.gameObject);

        foreach(var answeredQuestion in suspect.AnsweredQuestions)
            AskQuestion(answeredQuestion);

        ResetScrolling();
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
        newDialog.GetComponent<TextMeshProUGUI>().color = Color.magenta;

        var interrogationTextUI = newDialog.GetComponent<InterrogationTextUI>();
        textUIs.Add(interrogationTextUI);

        if (fromNPC)
            interrogationTextUI.Init(null, !fromNPC);
    }

    void AddStatement(Statement statement)
    {
        var newDialog = Instantiate(dialogTextPrefab, dialogParent.transform);

        var interrogationTextUI = newDialog.GetComponent<InterrogationTextUI>();
        textUIs.Add(interrogationTextUI);
        interrogationTextUI.Init(statement, false);

        interrogationTextUI.GetComponent<Button>().onClick.AddListener(delegate { OnStatementSelected(interrogationTextUI); });

        if (statement.AddToEvidence && !Inventory.i.HasStatement(statement))
            Instantiate(statementPrefab).GetComponent<StatementEvidence>().Init(statement, currentSuspect);
    }


    public void OnStatementSelected(InterrogationTextUI interrogationText)
    {
        selectedStatement = interrogationText;
        StartCoroutine(OnStatementSelectedAsync());
    }

    IEnumerator OnStatementSelectedAsync()
    {
        if (selectedStatement == null)
            yield break;

        if (selectedStatement.CurrentStatement == null)
            yield break;

        yield return GameController.i.StateMachine.PushAndWait(InventoryState.i);

        if (InventoryState.i.HasSelectedEvidence)
        {
            ResetScrolling();

            if(InventoryState.i.SelectedEvidence is StatementEvidence)
                AddText("The statement was shown.", false);
            else
                AddText(InventoryState.i.SelectedEvidence.Name + " was shown.", false);

            AddText(selectedStatement.CurrentStatement.StatementOnEvidence(InventoryState.i.SelectedEvidence), true);
        }
    }

    public void HandleScrolling()
    {
        //The vertical layout group of the interrogation box
        VerticalLayoutGroup dialogContainer = dialogParent.GetComponent<VerticalLayoutGroup>();

        //Get the current scroll position of the interrogation box
        var currentScrollPos = dialogParent.anchoredPosition.y;

        //Update the scroll position based on mouse wheel input
        var updatedScrollPos = currentScrollPos -= Input.mouseScrollDelta.y * scrollSpeed;

        //The amount to offset the maximum scroll distance to always keep at least one question/statement combo viewable
        float textOffset = (textUIs.Count != 0) ? textUIs[0].Height + textUIs[1].Height + dialogContainer.spacing * 4 + dialogContainer.padding.top + dialogContainer.padding.bottom: 0;

        //The maximum amount the interrogation box can scroll down
        float maxScrollPos = -dialogParent.rect.height + textOffset;

        if(maxScrollPos > 0)
            maxScrollPos = 0;

        //Clamp the scroll position between 0 and the height of the statement container
        updatedScrollPos = Mathf.Clamp(updatedScrollPos, maxScrollPos, 0);

        //Apply the updated scroll position
        dialogParent.anchoredPosition = new Vector2(0.5f, updatedScrollPos);
    }

    public void ResetScrolling()
    {
        dialogParent.anchoredPosition = new Vector2(0.5f, 0);
    }
}

[Serializable]
public class Question
{
    [SerializeField] string questionText;
    [SerializeField] Evidence requiredEvidence;
    [SerializeField] Statement response;

    public string QuestionText => questionText;
    public Evidence RequiredEvidence => requiredEvidence;
    public Statement Response => response;
}

[Serializable]
public class Statement
{
    [SerializeField] string statement;
    [SerializeField] Evidence disprovingEvidence;
    [SerializeField] string onCorrectEvidence;
    [SerializeField] string onWrongEvidence;

    [SerializeField] bool addStatementAsEvidence;

    public string Dialog => statement;
    public bool AddToEvidence => addStatementAsEvidence;

    public string StatementOnEvidence(Evidence evidence)
    {
        if (disprovingEvidence == null)
        {
            Reliability.i.AffectReliability(-15);
            return onWrongEvidence;
        }
        if (disprovingEvidence == evidence)
        {
            Reliability.i.AffectReliability(22);
            return onCorrectEvidence;
        }
        else
        {
            Reliability.i.AffectReliability(-15);
            return onWrongEvidence;
        }
    }
}
