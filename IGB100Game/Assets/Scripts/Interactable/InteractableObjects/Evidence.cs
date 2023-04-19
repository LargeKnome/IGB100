using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evidence : MonoBehaviour, Interactable
{
    [SerializeField] string itemDescription;

    public string Name => name;
    public string Description => itemDescription;

    public static Material detectivisionMat;

    Material defaultMat;
    public Material DefaultMat => defaultMat;

    void Start()
    {
        defaultMat = GetComponent<MeshRenderer>().material;
        GameController.i.Player.OnVisionActivate += UpdateMaterial;
    }

    void UpdateMaterial(bool activated)
    {
        gameObject.GetComponent<MeshRenderer>().material = (activated) ? detectivisionMat : defaultMat;
    }

    public IEnumerator Interact()
    {
        yield return DialogManager.i.ShowLine(itemDescription);
        yield return DialogManager.i.ShowLine("I better take this with me.");
        GameController.i.Player.Inventory.AddEvidence(this);

        gameObject.SetActive(false);
        yield return null;
    }
}
