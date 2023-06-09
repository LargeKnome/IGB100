using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionUI : MonoBehaviour
{
    [SerializeField] GameObject questionTextPrefab;
    [SerializeField] GameObject layoutGroup;

    public void Init(NPCController suspect)
    {
        var questions = suspect.InterrogationQuestions;

        foreach(Transform child in layoutGroup.transform)
            Destroy(child.gameObject);

        foreach(Question question in questions)
        {
            if (suspect.AnsweredQuestions.Contains(question))
                continue;

            if (question.RequiredEvidence != null)
            {
                if (!Inventory.i.HasEvidence(question.RequiredEvidence))
                    continue;
            }

            var questionText = Instantiate(questionTextPrefab, layoutGroup.transform);
            var questionScript = questionText.GetComponent<QuestionTextUI>();
            questionScript.Init(question);

            questionText.GetComponent<Button>().onClick.AddListener(delegate { InterrogationState.i.AskQuestion(questionScript.CurrentQuestion);});
        }
    }
}
