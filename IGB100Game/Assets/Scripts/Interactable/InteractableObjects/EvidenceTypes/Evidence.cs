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

    protected void UpdateMaterial(bool activated)
    {
        gameObject.GetComponent<MeshRenderer>().material = (activated) ? detectivisionMat : defaultMat;
    }
}
