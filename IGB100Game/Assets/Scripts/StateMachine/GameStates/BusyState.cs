using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusyState : State<GameController>
{
    public static BusyState i { get; private set; }

    private void Awake()
    {
        i = this;
    }
}
