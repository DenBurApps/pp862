using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class PauseScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _score, _timer, _lives;
    [SerializeField] private Button _resumeButton, _retryButton, _exitButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action ResumeClicked;
    public event Action RetryClicked;
    public event Action ExitClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(OnResumeClicked);
        _retryButton.onClick.AddListener(OnRetryClicked);
        _exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(OnResumeClicked);
        _retryButton.onClick.RemoveListener(OnRetryClicked);
        _exitButton.onClick.RemoveListener(OnExitClicked);
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void EnableScreen(int score, string timer, int lives)
    {
        _screenVisabilityHandler.EnableScreen();

        _score.text = score.ToString();
        _timer.text = timer;
        _lives.text = lives.ToString();
    }

    private void OnResumeClicked()
    {
        ResumeClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnRetryClicked()
    {
        RetryClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnExitClicked()
    {
        ExitClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }
}