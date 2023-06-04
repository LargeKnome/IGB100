using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnCameraAction : CutsceneAction
{
    public override IEnumerator Play()
    {
        Camera cam = GameController.i.MainCamera;

        var defaultPos = GameController.i.Player.PrevCamPos;
        var defaultRot = GameController.i.Player.PrevCamRot;

        var prevPos = cam.transform.position;
        var prevRot = cam.transform.rotation;

        for (var t = 0f; t < 1f; t += Time.deltaTime * 4)
        {
            //Lerps between cameras previous position and new position.
            cam.transform.SetPositionAndRotation(Vector3.Lerp(prevPos, defaultPos, t), Quaternion.Lerp(prevRot, defaultRot, t));
            yield return null;
        }
    }
}
