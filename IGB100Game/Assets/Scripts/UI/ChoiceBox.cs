using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI question;
    [SerializeField] TextMeshProUGUI cancel;

    bool questionSelected = true;
    bool changeSelection;

    public void HandleUpdate()
    {
        var vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (vertical != 0 && changeSelection)
        {
            questionSelected = !questionSelected;
            changeSelection = false;
        }
        else if (vertical == 0)
            changeSelection = true;

        question.color = (questionSelected) ? Color.blue : Color.black;
        cancel.color = (!questionSelected) ? Color.blue : Color.black;

        if (Input.GetButtonDown("Interact"))
        {
            if(questionSelected)
                GameController.i.StateMachine.Push(InterrogationState.i);
            else
                GameController.i.StateMachine.Pop();
        }

        if (Input.GetButtonDown("Back"))
            GameController.i.StateMachine.Pop();
    }
}
