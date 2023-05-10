using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Evidence
{
    protected override void OnStart()
    {
        defaultMat = GetComponent<MeshRenderer>().material;

        GameController.i.Player.OnVisionActivate += UpdateMaterial;
    }
}
