using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;

    [SerializeField] Transform cameraPlace;

    Camera cam;

    private void Start()
    {
        cam = GameController.i.MainCamera;
    }

    public IEnumerator Interact()
    {
        GameController.i.StateMachine.Push(BusyState.i);

        var prevCamPos = cam.transform.position;
        var prevCamRot = cam.transform.rotation;

        yield return MoveCamera(cameraPlace.position, cameraPlace.rotation);

        yield return DialogManager.i.ShowDialog(dialog);

        yield return MoveCamera(prevCamPos, prevCamRot);

        GameController.i.StateMachine.Pop();
    }

    IEnumerator MoveCamera(Vector3 newCamPos, Quaternion newCamRot)
    {
        for(var t = 0f; t < 0.5f; t += Time.deltaTime)
        {
            cam.transform.SetPositionAndRotation(Vector3.Lerp(cam.transform.position, newCamPos, t), Quaternion.Lerp(cam.transform.rotation, newCamRot, t));
            yield return null;
        }
    }
}
