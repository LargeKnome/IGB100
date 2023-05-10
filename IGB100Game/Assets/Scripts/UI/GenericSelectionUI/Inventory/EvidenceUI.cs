using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EvidenceUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject evidenceModel;
    [SerializeField] float rotationSpeed;

    [SerializeField] Color selectedColor;
    [SerializeField] Color hoverColor;

    Color defaultColor;

    public event Action<EvidenceUI> onHoverEnter;

    public bool Selected { get; set; }

    Image background;

    public Evidence Evidence { get; private set; }

    public void Init(Evidence evidence)
    {
        Selected = false;
        background = GetComponent<Image>();
        defaultColor = background.color;

        Evidence = evidence;

        if (evidence is EvidenceObj evidenceObj)
        {
            evidenceModel.GetComponent<MeshFilter>().mesh = evidence.GetComponent<MeshFilter>().mesh;
            evidenceModel.GetComponent<MeshRenderer>().material = evidenceObj.DefaultMat;
            float sizeFactor = evidenceModel.transform.localScale.x / Mathf.Max(evidence.transform.localScale.x, evidence.transform.localScale.y, evidence.transform.localScale.z);
            evidenceModel.transform.localScale = evidence.transform.localScale * sizeFactor;
        }
        else if(evidence is Key key)
        {
            evidenceModel.GetComponent<MeshFilter>().mesh = evidence.GetComponent<MeshFilter>().mesh;
            evidenceModel.GetComponent<MeshRenderer>().material = key.DefaultMat;
            float sizeFactor = evidenceModel.transform.localScale.x / Mathf.Max(evidence.transform.localScale.x, evidence.transform.localScale.y, evidence.transform.localScale.z);
            evidenceModel.transform.localScale = evidence.transform.localScale * sizeFactor;
        }
    }

    public void HandleUpdate()
    {
        float timeDiff = Time.deltaTime * rotationSpeed;

        if (Evidence is EvidenceObj)
        {
            evidenceModel.transform.Rotate(Vector3.up, timeDiff);
            evidenceModel.transform.Rotate(Vector3.forward, timeDiff);
            evidenceModel.transform.Rotate(Vector3.right, timeDiff);
        }
    }

    public void SetSelected(bool selected)
    {
        Selected = selected;

        background.color = (Selected) ? selectedColor : hoverColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = (Selected) ? selectedColor : hoverColor;
        onHoverEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Selected) return;

        background.color = defaultColor;
    }
}
