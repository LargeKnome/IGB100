using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    Button button;
    State<GameController> CurrentState;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Exit);
    }

    private void OnEnable()
    {
        CurrentState = GameController.i.StateMachine.CurrentState;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && GameController.i.StateMachine.CurrentState == CurrentState)
            GameController.i.StateMachine.Pop();
    }

    private void Exit()
    {
        GameController.i.StateMachine.Pop();
    }
}
