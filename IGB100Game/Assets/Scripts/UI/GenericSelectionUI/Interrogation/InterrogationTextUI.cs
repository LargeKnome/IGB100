using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterrogationTextUI : MonoBehaviour, ISelectableItem
{
    public Statement CurrentStatement { get; private set; }

    public float Height { get; private set; }

    TextMeshProUGUI textMesh;
    public void Init(Statement statement)
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.color = Color.black;
        Height = textMesh.GetComponent<RectTransform>().rect.height;
        CurrentStatement = statement;

        if (statement != null)
            textMesh.text = statement.Dialog;
        
    }
    public void OnSelectionChanged(bool selected)
    {
        if (CurrentStatement == null)
            textMesh.color = (selected) ? Color.red : Color.black;
        else
            textMesh.color = (selected) ? Color.blue : Color.black;
    }
}
