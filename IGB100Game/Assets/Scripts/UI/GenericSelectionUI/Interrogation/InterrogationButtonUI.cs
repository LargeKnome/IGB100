using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterrogationButtonUI : MonoBehaviour, ISelectableItem
{
    [SerializeField] TextMeshProUGUI textMesh;

    public void OnSelectionChanged(bool selected)
    {
        textMesh.color = (selected) ? Color.blue : Color.black;
    }
}
