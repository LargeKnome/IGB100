using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamState : State<GameController>
{
    [SerializeField] FirstPersonController player;

    public static FreeRoamState i { get; private set; }

    private void Awake()
    {
        i = this;
    }

    public override void Execute()
    {
        player.HandleUpdate();
    }
}
