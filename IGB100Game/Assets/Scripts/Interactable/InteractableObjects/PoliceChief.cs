using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PoliceChief : MonoBehaviour, Interactable
{
    [SerializeField] Transform cameraPlace;

    [SerializeField] Evidence motive;
    [SerializeField] Evidence weapon;
    [SerializeField] NPCController murderer;

    [SerializeField] UnityEvent OnSuccessfulAccusation;

    Camera cam;

    private void Start()
    {
        cam = GameController.i.MainCamera;
    }

    public IEnumerator Interact()
    {
        //Saves prev camera transform to return to once interrogation is over
        var prevCamPos = cam.transform.position;
        var prevCamRot = cam.transform.rotation;

        //Moves the camera to face the NPC
        yield return MoveCamera(cameraPlace.position, cameraPlace.rotation);

        yield return DialogManager.i.ShowLine("What evidence do you have to show me?", false);

        yield return GameController.i.StateMachine.PushAndWait(AccusationState.i);

        if (!AccusationState.i.HasAccused)
        {
            yield return MoveCamera(prevCamPos, prevCamRot);
            yield break;
        }

        bool successfulAccusation = AccusationState.i.Suspect == murderer && AccusationState.i.Motive == motive && AccusationState.i.Weapon == weapon;

        if (successfulAccusation)
        {
            yield return DialogManager.i.ShowLine("Alright, I'm convinced. You solved the case, detective. well done.", false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            OnSuccessfulAccusation?.Invoke();
        }
        else
        {
            yield return DialogManager.i.ShowLine("I don't think I'm convinced.", false);
            Reliability.i.AffectReliability(-33);
        }

        //Move the camera to position it was in before interaction
        yield return MoveCamera(prevCamPos, prevCamRot);
    }

    /*
     * Moves the camera from its current position to new position over time
     */
    IEnumerator MoveCamera(Vector3 newCamPos, Quaternion newCamRot)
    {
        //Saves cameras current position
        var prevPos = cam.transform.position;
        var prevRot = cam.transform.rotation;

        for (var t = 0f; t < 1f; t += Time.deltaTime * 4)
        {
            //Lerps between cameras previous position and new position.
            cam.transform.SetPositionAndRotation(Vector3.Lerp(prevPos, newCamPos, t), Quaternion.Lerp(prevRot, newCamRot, t));
            yield return null;
        }
    }
}
