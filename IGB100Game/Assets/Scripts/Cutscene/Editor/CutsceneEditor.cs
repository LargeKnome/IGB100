using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Cutscene))]
public class CutsceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var cutscene = target as Cutscene;

        using (var scope = new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Dialog"))
                cutscene.AddAction(new DialogAction());
            else if (GUILayout.Button("Move Camera"))
                cutscene.AddAction(new MoveCameraAction());
            else if (GUILayout.Button("Return Camera"))
                cutscene.AddAction(new ReturnCameraAction());
            else if (GUILayout.Button("Add Event"))
                cutscene.AddAction(new EventAction());
        };

        base.OnInspectorGUI();
    }
}