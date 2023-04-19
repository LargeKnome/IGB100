using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI refute;
    [SerializeField] TextMeshProUGUI cancel;

    bool refuteSelected = true;
    bool changeSelection;

    public bool Refute => refuteSelected;

    public void HandleUpdate()
    {
        var vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (vertical != 0 && changeSelection)
        {
            refuteSelected = !refuteSelected;
            changeSelection = false;
        }
        else if (vertical == 0)
            changeSelection = true;

        refute.color = (refuteSelected) ? Color.blue : Color.black;
        cancel.color = (!refuteSelected) ? Color.blue : Color.black;

        if (Input.GetButtonDown("Interact"))
            GameController.i.StateMachine.Pop();

        if (Input.GetButtonDown("Back"))
        {
            refuteSelected = false;
            GameController.i.StateMachine.Pop();
        }
    }
}
