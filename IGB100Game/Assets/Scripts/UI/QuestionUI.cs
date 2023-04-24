using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionUI : MonoBehaviour
{
    [SerializeField] GameObject questionTextPrefab;
    [SerializeField] GameObject layoutGroup;

    int selectedQuestion;
    bool changeSelection;

    List<Question> questions;
    List<TextMeshProUGUI> questionTexts;

    public void Init(List<Question> possibleQuestions)
    {
        selectedQuestion = 0;
        questions = possibleQuestions;

        foreach(Transform child in layoutGroup.transform)
            Destroy(child.gameObject);

        questionTexts = new List<TextMeshProUGUI>();

        foreach(Question question in possibleQuestions)
        {
            var questionText = Instantiate(questionTextPrefab, layoutGroup.transform);
            questionText.GetComponent<TextMeshProUGUI>().text = question.QuestionText;
            questionTexts.Add(questionText.GetComponent<TextMeshProUGUI>());
        }

        questionTexts[selectedQuestion].color = Color.yellow;
    }

    public void HandleUpdate()
    {
        var prevInput = selectedQuestion;
        var input = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        if (input != 0 && changeSelection)
        {
            selectedQuestion -= input;
            changeSelection = false;
        }
        else if (input == 0)
            changeSelection = true;

        selectedQuestion = Mathf.Clamp(selectedQuestion, 0, questions.Count - 1);

        if (prevInput != selectedQuestion)
        {
            questionTexts[prevInput].color = Color.white;
            questionTexts[selectedQuestion].color = Color.yellow;
        }

        if (Input.GetButtonDown("Interact"))
        {
            InterrogationState.i.AskQuestion(questions[selectedQuestion]);
            GameController.i.StateMachine.Pop();
        }

        if (Input.GetButtonDown("Back"))
            GameController.i.StateMachine.Pop();
    }
}
