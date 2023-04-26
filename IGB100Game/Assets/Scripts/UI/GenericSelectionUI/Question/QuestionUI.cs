using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionUI : SelectionUI<QuestionTextUI>
{
    [SerializeField] GameObject questionTextPrefab;
    [SerializeField] GameObject layoutGroup;

    List<QuestionTextUI> questionTexts;

    public void Init(NPCController suspect)
    {
        questionTexts = new();
        var questions = suspect.InterrogationQuestions;

        foreach(Transform child in layoutGroup.transform)
            Destroy(child.gameObject);

        foreach(Question question in questions)
        {
            if (suspect.AnsweredQuestions.Contains(question))
                continue;
            if (question.RequiredEvidence != null)
            {
                if (!GameController.i.Player.Inventory.EvidenceList.Contains(question.RequiredEvidence))
                    continue;
            }

            var questionText = Instantiate(questionTextPrefab, layoutGroup.transform);
            questionText.GetComponent<QuestionTextUI>().Init(question);
            questionTexts.Add(questionText.GetComponent<QuestionTextUI>());
        }

        SetItems(questionTexts, 1);
    }
}
