using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionTextUI : MonoBehaviour, ISelectableItem
{
    public Question CurrentQuestion { get; private set; }

    TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void Init(Question question)
    {
        CurrentQuestion = question;
        textMesh.text = question.QuestionText;
    }

    public void OnSelectionChanged(bool selected)
    {
        textMesh.color = (selected) ? Color.yellow : Color.white;
    }
}
