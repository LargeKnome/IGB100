using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Evidence : MonoBehaviour
{
    [SerializeField] protected string[] itemDescription;

    public string Name => name;
    public string[] Description => itemDescription;

    protected Material defaultMat;
    protected List<Material> defaultMats;

    List<GameObject> children;

    public Material DefaultMat => defaultMat;
    public List<Material> DefaultMats => defaultMats;

    public List<GameObject> Children => children;

    protected virtual void OnStart()
    {

    }

    void Start()
    {
        OnStart();
    }

    protected void SetUpMats()
    {
        if (GetComponent<MeshRenderer>() != null)
            defaultMat = GetComponent<MeshRenderer>().material;
        else
        {
            children = new List<GameObject>();

            defaultMats = new List<Material>();

            foreach (Transform child in transform)
            {
                children.Add(child.gameObject);
                defaultMats.Add(child.GetComponent<MeshRenderer>().material);
            }
        }

        GameController.i.Player.OnVisionActivate += UpdateMaterial;
    }

    protected IEnumerator OnPickup()
    {
        foreach (string line in itemDescription)
            yield return DialogManager.i.ShowLine(line, true);
        yield return DialogManager.i.ShowLine("I better take this with me.", false);
        
        Inventory.i.AddEvidence(this);

        InspectionState.i.TempEvidence = this;
        yield return GameController.i.StateMachine.PushAndWait(InspectionState.i);

        Reliability.i.AffectReliability(5);

        gameObject.SetActive(false);
        yield return null;
    }

    protected void UpdateMaterial(bool activated)
    {
        if (gameObject.GetComponent<MeshRenderer>() != null)
            gameObject.GetComponent<MeshRenderer>().material = (activated) ? GameController.i.Player.DetectivisionMat : defaultMat;
        else
        {
            int i = 0;
            foreach (Transform child in transform)
            {
                child.GetComponent<MeshRenderer>().material = (activated) ? GameController.i.Player.DetectivisionMat : defaultMats[i];
                i++;
            }
        }
    }
}
