using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneState : State<GameController>
{
    public static CutsceneState i;

    private void Awake()
    {
        i = this;
    }
}
