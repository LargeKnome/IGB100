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
    [SerializeField] GameObject dialogParent;
    [SerializeField] List<TextMeshProUGUI> buttons;
    [SerializeField] GameObject dialogTextPrefab;

    NPCController currentSuspect;
    Statement currentStatement;

    int selectedButton;
    bool changeSelection;

    public void Init(NPCController suspect)
    {
        currentSuspect = suspect;
        characterName.text = suspect.name;

        selectedButton = 0;

        foreach (Transform textObj in dialogParent.transform)
            Destroy(textObj.gameObject);

        foreach (var button in buttons)
            button.color = Color.black;

        buttons[selectedButton].color = Color.blue;
    }

    public void HandleUpdate()
    {
        var prevInput = selectedButton;
        var input = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (input != 0 && changeSelection)
        {
            selectedButton -= input;
            changeSelection = false;
        }
        else if (input == 0)
            changeSelection = true;

        selectedButton = Mathf.Clamp(selectedButton, 0, buttons.Count - 1);

        if(prevInput != selectedButton)
        {
            buttons[prevInput].color = Color.black;
            buttons[selectedButton].color = Color.blue;
        }

        if(Input.GetButtonDown("Interact"))
            StartCoroutine(HandleSelection());

        if (Input.GetButtonDown("Back"))
            GameController.i.StateMachine.Pop();
    }

    public void AskQuestion(Question question)
    {
        AddText(question.QuestionText, false);
        currentStatement = question.Response;
        AddText(question.Response.Dialog, true);
    }

    void AddText(string text, bool fromNPC)
    {
        var newDialog = Instantiate(dialogTextPrefab, dialogParent.transform);

        newDialog.GetComponent<TextMeshProUGUI>().text = text;
        newDialog.GetComponent<TextMeshProUGUI>().color = (fromNPC) ? Color.black : Color.magenta;
    }

    IEnumerator HandleSelection()
    {
        if (selectedButton == 0) // Question
        {
            yield return GameController.i.StateMachine.PushAndWait(QuestionState.i);
        }
        else if (selectedButton == 1) //Refute
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
        else if (selectedButton == 2) //Exit
            GameController.i.StateMachine.Pop();

        yield return null;
    }
}
