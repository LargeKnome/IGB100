using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceUI : MonoBehaviour, ISelectableItem
{
    [SerializeField] GameObject evidenceObj;
    [SerializeField] float rotationSpeed;

    public Evidence Evidence { get; private set; }

    public void Init(Evidence evidence)
    {
        Evidence = evidence;
        evidenceObj.GetComponent<MeshFilter>().mesh = evidence.GetComponent<MeshFilter>().mesh;
        evidenceObj.GetComponent<MeshRenderer>().material = evidence.DefaultMat;

        float sizeFactor = evidenceObj.transform.localScale.x/Mathf.Max(evidence.transform.localScale.x, evidence.transform.localScale.y, evidence.transform.localScale.z);
        evidenceObj.transform.localScale = evidence.transform.localScale * sizeFactor;
    }


    public void HandleUpdate()
    {
        float timeDiff = Time.deltaTime * rotationSpeed;

        //Rotates the object over time
        evidenceObj.transform.Rotate(Vector3.up, timeDiff);
        evidenceObj.transform.Rotate(Vector3.forward, timeDiff);
        evidenceObj.transform.Rotate(Vector3.right, timeDiff);
    }

    public void OnSelectionChanged(bool selected)
    {
        GetComponent<Image>().enabled = selected;
    }
}