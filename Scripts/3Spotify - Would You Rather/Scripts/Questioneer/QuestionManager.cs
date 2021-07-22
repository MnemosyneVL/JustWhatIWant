using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField]
    private QuestionItem[] _questions = default;

    [SerializeField]
    private int _currentQuestionNr = 0;

    [Header("References")]
    [SerializeField]
    private Image _backgroundImage = default;

    [SerializeField]
    private Image _lineImage = default;

    [SerializeField]
    private Text _questionText = default;

    [SerializeField]
    private Text _answer1 = default;
    [SerializeField]
    private Image _picture1 = default;

    [SerializeField]
    private Text _answer2 = default;
    [SerializeField]
    private Image _picture2 = default;

    [SerializeField]
    private Transform _progressBarObj = default;

    [SerializeField]
    private EventSystem _eventSystem = default;

    [Header("Settings")]
    [SerializeField]
    private Image _progressPointPrefab = default;

    //Other Fields

    private List<Image> _progressPoints = new List<Image>();

    #region Interface
    public void NextQuestion()
    {
        _eventSystem.SetSelectedGameObject(null);
        if (_currentQuestionNr + 1 >= _questions.Length)
        {
            Debug.LogWarning("No more questions left!");
            SceneController.instance.ChangeScene(3);
            return;
        }
        LoadQuestionData(_currentQuestionNr + 1);
    }
    #endregion

    private void CreateProgressPoints()
    {
        for (int i = 0; i < _questions.Length; i++)
        {
            Image progressPoint = Instantiate(_progressPointPrefab, _progressBarObj);
            _progressPoints.Add(progressPoint);
        }
    }

    private void LoadQuestionData(int questionNr)
    {
        _currentQuestionNr = questionNr;
        _questionText.text = _questions[_currentQuestionNr].question;
        _answer1.text = _questions[_currentQuestionNr].answer1;
        _answer2.text = _questions[_currentQuestionNr].answer2;
        _picture1.sprite = _questions[_currentQuestionNr].picture1;
        _picture2.sprite = _questions[_currentQuestionNr].picture2;
        SetNewColors(_questions[_currentQuestionNr]);

        foreach (Image img in _progressPoints)
        {
            Color currentColor = img.color;
            img.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.2f);
        }

        for (int i = 0; i <= _currentQuestionNr; i++)
        {
            Color currentColor = _progressPoints[i].color;
            _progressPoints[i].color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.7f);
        }

    }

    private void SetNewColors(QuestionItem questionItem)
    {
        _backgroundImage.color = questionItem.backgroundColor;
        _lineImage.color = questionItem.generalColor;
        _answer1.color = questionItem.generalColor;
        _answer2.color = questionItem.generalColor;
        _picture1.color = questionItem.generalColor;
        _picture2.color = questionItem.generalColor;
        _questionText.color = questionItem.generalColor;
        foreach (Image point in _progressPoints)
        {
            point.color = questionItem.generalColor;
        }
    }

    private void Start()
    {
        CreateProgressPoints();
        LoadQuestionData(0);
    }


}
