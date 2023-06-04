using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCameraAction : CutsceneAction
{
    [SerializeField] Transform cameraMoveTo;

    public override IEnumerator Play()
    {
        Camera cam = GameController.i.MainCamera;

        GameController.i.Player.PrevCamPos = cam.transform.position;
        GameController.i.Player.PrevCamRot = cam.transform.rotation;

        var prevPos = GameController.i.Player.PrevCamPos;
        var prevRot = GameController.i.Player.PrevCamRot;

        for (var t = 0f; t < 1f; t += Time.deltaTime * 4)
        {
            //Lerps between cameras previous position and new position.
            cam.transform.SetPositionAndRotation(Vector3.Lerp(prevPos, cameraMoveTo.position, t), Quaternion.Lerp(prevRot, cameraMoveTo.rotation, t));
            yield return null;
        }
    }
}
