using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InterrogationTextUI : GenericButton
{
    public bool IsPlayerStatement { get; private set; } = true;

    public Statement CurrentStatement { get; private set; }

    public float Height { get; private set; }

    public void Init(Statement statement, bool fromPlayer)
    {
        IsPlayerStatement = fromPlayer;

        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.color = Color.black;
        CurrentStatement = statement;

        if (statement != null)
            textMesh.text = statement.Dialog;

        Height = textMesh.GetComponent<RectTransform>().rect.height;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(!IsPlayerStatement)
            textMesh.color = (CurrentStatement == null) ? Color.red : selectedColor;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if(!IsPlayerStatement)
            base.OnPointerExit(eventData);
    }
}
