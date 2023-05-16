using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EvidenceUI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] GameObject evidenceModel;
    [SerializeField] float rotationSpeed;

    public event Action<EvidenceUI> onHoverEnter;

    public bool Selected { get; set; }

    Image background;

    public Evidence Evidence { get; private set; }

    public void Init(Evidence evidence)
    {
        Selected = false;
        background = GetComponent<Image>();

        if(evidence == null)
        {
            evidenceModel.SetActive(false);
            return;
        }

        Evidence = evidence;

        if (evidence is EvidenceObj || evidence is Key)
        {
            evidenceModel.SetActive(true);
            evidenceModel.GetComponent<MeshFilter>().mesh = evidence.GetComponent<MeshFilter>().mesh;
            evidenceModel.GetComponent<MeshRenderer>().material = evidence.DefaultMat;
            float sizeFactor = evidenceModel.transform.localScale.x / Mathf.Max(evidence.transform.localScale.x, evidence.transform.localScale.y, evidence.transform.localScale.z);
            evidenceModel.transform.localScale = evidence.transform.localScale * sizeFactor;
        }
        else if(evidence is StatementEvidence || evidence is NPCController)
        {
            evidenceModel.SetActive(false);
        }
    }

    public void HandleUpdate()
    {
        float timeDiff = Time.deltaTime * rotationSpeed;

        if (Evidence is EvidenceObj || Evidence is Key)
        {
            evidenceModel.transform.Rotate(Vector3.up, timeDiff);
            evidenceModel.transform.Rotate(Vector3.forward, timeDiff);
            evidenceModel.transform.Rotate(Vector3.right, timeDiff);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHoverEnter?.Invoke(this);
    }
}
