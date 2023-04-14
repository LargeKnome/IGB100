using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public StateMachine<GameController> StateMachine { get; private set; }

    public static GameController i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    void Start()
    {
        StateMachine = new(this);
        StateMachine.Push(FreeRoamState.i);
    }

    void Update()
    {
        StateMachine.Execute();
    }
}
