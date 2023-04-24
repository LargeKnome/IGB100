using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [Header("Default Dialog")]
    [SerializeField] string startingSentence;
    [SerializeField] List<Statement> statements;

    [SerializeField] Transform cameraPlace;

    Camera cam;

    float camMoveSpeed = 4f;

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

        yield return DialogManager.i.ShowLine(startingSentence);

        yield return DialogManager.i.ShowDialog(statements);

        yield return MoveCamera(prevCamPos, prevCamRot);

        GameController.i.StateMachine.Pop();
    }

    IEnumerator MoveCamera(Vector3 newCamPos, Quaternion newCamRot)
    {
        var prevPos = cam.transform.position;
        var prevRot = cam.transform.rotation;

        for(var t = 0f; t < 1f; t += Time.deltaTime * camMoveSpeed)
        {
            cam.transform.SetPositionAndRotation(Vector3.Lerp(prevPos, newCamPos, t), Quaternion.Lerp(prevRot, newCamRot, t));
            yield return null;
        }
    }
}
