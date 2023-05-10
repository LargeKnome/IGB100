using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Evidence : MonoBehaviour, Interactable
{
    [SerializeField] string[] itemDescription;

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

    protected void UpdateMaterial(bool activated)
    {
        gameObject.GetComponent<MeshRenderer>().material = (activated) ? detectivisionMat : defaultMat;
    }

    public IEnumerator Interact()
    {
        foreach (string line in itemDescription)
            yield return DialogManager.i.ShowLine(line);
        yield return DialogManager.i.ShowLine("I better take this with me.");
        GameController.i.Player.Inventory.AddEvidence(this);

        gameObject.SetActive(false);
        yield return null;
    }
}
