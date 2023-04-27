using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    //List of questions the player can ask the NPC during interrogation
    [SerializeField] List<Question> interrogationQuestions;
    public List<Question> InterrogationQuestions => interrogationQuestions;

    //List of questions the NPC has already answered
    public List<Question> AnsweredQuestions { get; set; }

    //The location to move the camera to when interacting with NPC
    [SerializeField] Transform cameraPlace;

    Camera cam;

    const float camMoveSpeed = 4f;

    private void Start()
    {
        cam = GameController.i.MainCamera;
        AnsweredQuestions = new List<Question>();
    }

    /*
     * Pushes the interrogation state when player interacts,
     * moving the camera to face the NPC and
     * moving the camera back once the interrogation state has exited
     */
    public IEnumerator Interact()
    {
        //Sets the NPC as the one being interrogated
        InterrogationState.i.SetSuspect(this);

        //Pushes busy state so player can't move when camera is moving
        GameController.i.StateMachine.Push(BusyState.i);

        //Saves prev camera transform to return to once interrogation is over
        var prevCamPos = cam.transform.position;
        var prevCamRot = cam.transform.rotation;

        //Moves the camera to face the NPC
        yield return MoveCamera(cameraPlace.position, cameraPlace.rotation);

        //Activates the interrogation state and waits until the interrogation is over.
        yield return GameController.i.StateMachine.PushAndWait(InterrogationState.i);

        //Move the camera to position it was in before interaction
        yield return MoveCamera(prevCamPos, prevCamRot);

        //Goes back to free roam state for player movement
        GameController.i.StateMachine.Pop();
    }

    /*
     * Moves the camera from its current position to new position over time
     */
    IEnumerator MoveCamera(Vector3 newCamPos, Quaternion newCamRot)
    {
        //Saves cameras current position
        var prevPos = cam.transform.position;
        var prevRot = cam.transform.rotation;

        for(var t = 0f; t < 1f; t += Time.deltaTime * camMoveSpeed)
        {
            //Lerps between cameras previous position and new position.
            cam.transform.SetPositionAndRotation(Vector3.Lerp(prevPos, newCamPos, t), Quaternion.Lerp(prevRot, newCamRot, t));
            yield return null;
        }
    }
}