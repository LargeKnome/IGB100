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

    int selectedButton;
    bool changeSelection;

    public void Init(NPCController suspect)
    {
        characterName.text = suspect.name;

        selectedButton = 0;

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
            HandleSelection();

        if (Input.GetButtonDown("Back"))
            GameController.i.StateMachine.Pop();
    }

    void HandleSelection()
    {
        if (selectedButton == 0) // Question
        {

        }
        else if (selectedButton == 1) //Refute
        {

        }
        else if (selectedButton == 2) //Exit
            GameController.i.StateMachine.Pop();
    }
}
