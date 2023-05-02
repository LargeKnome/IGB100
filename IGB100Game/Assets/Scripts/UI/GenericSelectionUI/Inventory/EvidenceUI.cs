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
    [SerializeField] GameObject evidenceObj;
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
