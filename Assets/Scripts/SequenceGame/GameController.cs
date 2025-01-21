using System.Collections;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SequenceGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _livesText;
        [SerializeField] private StarHolder _elementHolder;
        [SerializeField] private GameOverScreen _loseScreen;
        [SerializeField] private GameOverScreen _winScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private CountdownScreen _countdownScreen;
        [SerializeField] private GameType _gameType;
        [SerializeField] private TutorialScreen _tutorialScreen;
        [SerializeField] private GameObject _disableObject;

        private readonly int _startDifficulty = 3;
        private int _difficulty;
        private int _lives;
        private int _score;
        private float _timer;
        private IEnumerator _timerCoroutine;

        private void OnEnable()
        {
            _tutorialScreen.PlayClicked += ShowSequence;
            
            _countdownScreen.Disabled += StartNewGame;

            _loseScreen.ExitClicked += ExitGame;
            _loseScreen.RetryClicked += Restart;
            
            _winScreen.ExitClicked += ExitGame;
            _winScreen.RetryClicked += Restart;
            
            _pauseButton.onClick.AddListener(PauseGame);
            _pauseScreen.ExitClicked += ExitGame;
            _pauseScreen.RetryClicked += Restart;
            _pauseScreen.ResumeClicked += ContinueGame;

            _elementHolder.AllElementsShown += () => _countdownScreen.Disable();
            _elementHolder.ElementCorrectlyChosen += ElementCorrectlyChosen;
            _elementHolder.ElementIncorrectlyChosen += ElementIncorrectlyChosen;
            _elementHolder.AllElementsCorrectlyChosen += GameWin;
        }

        private void OnDisable()
        {
            _tutorialScreen.PlayClicked -= ShowSequence;
            
            _countdownScreen.Disabled -= StartNewGame;

            _loseScreen.ExitClicked -= ExitGame;
            _loseScreen.RetryClicked -= Restart;
            
            _winScreen.ExitClicked -= ExitGame;
            _winScreen.RetryClicked -= Restart;

            _pauseButton.onClick.RemoveListener(PauseGame);
            _pauseScreen.ExitClicked -= ExitGame;
            _pauseScreen.RetryClicked -= Restart;
            _pauseScreen.ResumeClicked -= ContinueGame;

            _elementHolder.AllElementsShown -= () => _countdownScreen.Disable();
            _elementHolder.ElementCorrectlyChosen -= ElementCorrectlyChosen;
            _elementHolder.ElementIncorrectlyChosen -= ElementIncorrectlyChosen;
            _elementHolder.AllElementsCorrectlyChosen -= GameWin;
        }

        private void Start()
        {
            _difficulty = _startDifficulty;
            _elementHolder.DisableAllElements();
            ResetAllValues();
            _countdownScreen.DisableScreen();
            
            if (GameLoader.IsTutorialMode)
            {
                int difficulty = GameLoader.SelectedDifficulty;
                _difficulty = difficulty;
                _tutorialScreen.gameObject.SetActive(false);
                ShowSequence();
            }
        }

        private void ShowSequence()
        {
            _elementHolder.EnableAllElements();
            _elementHolder.SetSequenceCount(_difficulty);
            _elementHolder.ResumeSequence();
            _elementHolder.StartSequence();
        }
        

        private void StartNewGame()
        {
            _elementHolder.ActivateAllElements();
            StartTimerCoroutine();
        }
        
        private void Restart()
        {
            ResetAllValues();
            _elementHolder.DisableAllElements();
            ShowSequence();
            UpdateUIText();
        }

        private void UpdateUIText()
        {
            _scoreText.text = _score.ToString();
            _livesText.text = _lives.ToString();
        }

        private void ResetAllValues()
        {
            _lives = 3;
            _score = 0;
            _timer = 0;
        }

        private void ElementCorrectlyChosen()
        {
            _score += 100;
            UpdateUIText();
        }

        private void ElementIncorrectlyChosen()
        {
            _lives--;
            UpdateUIText();

            if (_lives <= 0)
            {
                GameLost();
            }
        }

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(_timer / 60);
            int seconds = Mathf.FloorToInt(_timer % 60);

            _timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void StartTimerCoroutine()
        {
            StopTimerCoroutine();

            _timerCoroutine = StartTimer();
            StartCoroutine(_timerCoroutine);
        }

        private void StopTimerCoroutine()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
        }

        private IEnumerator StartTimer()
        {
            while (true)
            {
                _timer += Time.deltaTime;
                UpdateTimerText();
                yield return null;
            }
        }

        private void GameLost()
        {
            _loseScreen.EnableScreen(_score, _timerText.text, _lives);
        }

        private void GameWin()
        {
            _winScreen.EnableScreen(_score, _timerText.text, _lives);
            RecordHolder.AddNewRecord(_gameType, _score, _timer);

            if (!GameLoader.IsTutorialMode)
            {
                _difficulty++;
            }
        }

        private void PauseGame()
        {
            _elementHolder.PauseSequence();
            _pauseScreen.EnableScreen(_score, _timerText.text, _lives);
        }

        private void ContinueGame()
        {
            _elementHolder.ResumeSequence();
        }

        private void ExitGame()
        {
            if (!GameLoader.IsTutorialMode)
                RecordHolder.AddNewRecord(_gameType, _score, _timer);
            
            _disableObject.SetActive(true);
            GameLoader.ResetTutorialMode();
            SceneManager.LoadScene("MainScene");
        }
    }
}