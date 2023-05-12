using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenericButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected TextMeshProUGUI textMesh;

    [SerializeField] protected Color selectedColor;
    [SerializeField] protected Color unselectedColor;

    public void UpdateSelection(bool selected)
    {
        textMesh.color = (selected) ? selectedColor : unselectedColor;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        textMesh.color = selectedColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        textMesh.color = unselectedColor;
    }
}
