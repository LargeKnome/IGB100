using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Evidence : MonoBehaviour
{
    [SerializeField] protected string[] itemDescription;

    public string Name => name;
    public string[] Description => itemDescription;


    public static Material detectivisionMat;

    protected Material defaultMat;
    public Material DefaultMat => defaultMat;

    protected virtual void OnStart()
    {

    }

    void Start()
    {
        OnStart();
    }

    protected IEnumerator OnPickup()
    {
        foreach (string line in itemDescription)
            yield return DialogManager.i.ShowLine(line);
        yield return DialogManager.i.ShowLine("I better take this with me.");
        
        Inventory.i.AddEvidence(this);

        InspectionState.i.TempEvidence = this;
        yield return GameController.i.StateMachine.PushAndWait(InspectionState.i);

        Reliability.i.AffectReliability(5);

        gameObject.SetActive(false);
        yield return null;
    }

    protected void UpdateMaterial(bool activated)
    {
        gameObject.GetComponent<MeshRenderer>().material = (activated) ? detectivisionMat : defaultMat;
    }
}
