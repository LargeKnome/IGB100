using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] FirstPersonController player;

    [SerializeField] Material detectivisionMat;

    public Camera MainCamera => mainCamera;
    public FirstPersonController Player => player;

    public StateMachine<GameController> StateMachine { get; private set; }

    public static GameController i { get; private set; }

    private void Awake()
    {
        i = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
