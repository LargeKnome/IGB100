using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefuteUI : SelectionUI<InterrogationTextUI>
{
    List<InterrogationTextUI> statementUIs;

    RectTransform rectTransform;
    VerticalLayoutGroup verticalLayoutGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
    }
    public void Init(List<InterrogationTextUI> statements)
    {
        statementUIs = statements;

        SetItems(statementUIs, 1);
    }

    public override void OnSelectionChanged(bool onInit)
    {
        base.OnSelectionChanged(onInit);

        HandleScrolling();
    }

    void HandleScrolling()
    {
        float scrollPos = (items.Count - 1 - currentSelection) * (statementUIs[0].Height + verticalLayoutGroup.spacing) * 2;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -scrollPos);
    }

    public void ResetScrolling()
    {
        rectTransform.anchoredPosition = new Vector2(0.5f, -0);
    }
}
