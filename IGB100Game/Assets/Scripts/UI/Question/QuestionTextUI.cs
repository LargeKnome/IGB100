using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionTextUI : GenericButton
{
    public Question CurrentQuestion { get; private set; }

    public void Init(Question question)
    {
        CurrentQuestion = question;
        textMesh.text = question.QuestionText;
    }
}
