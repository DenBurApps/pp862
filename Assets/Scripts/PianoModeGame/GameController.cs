using System;
using System.Collections;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PianoModeGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _livesText;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private CountdownScreen _countdownScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private TutorialScreen _tutorialScreen;
        [SerializeField] private GameOverScreen _gameOverScreen;
        [SerializeField] private Player _player;
        [SerializeField] private SquareSpawner _squareSpawner;
        [SerializeField] private GameType _gameType;
        [SerializeField] private GameObject _disableObject;

        private int _score;
        private float _timer;
        private int _lives;
        private IEnumerator _timerCoroutine;

        private void Start()
        {
            _countdownScreen.DisableScreen();
            
            if (GameLoader.IsTutorialMode)
            {
                int difficulty = GameLoader.SelectedDifficulty;
                _squareSpawner.SetMovingSpeed(difficulty);
                _tutorialScreen.gameObject.SetActive(false);
                _countdownScreen.EnableScreen();
                _countdownScreen.Disable();
            }
        }

        private void OnEnable()
        {
            _countdownScreen.Disabled += StartNewGame;

            _player.Touched += OnTouched;

            _gameOverScreen.ExitClicked += ExitGame;
            _gameOverScreen.RetryClicked += Restart;

            _pauseButton.onClick.AddListener(PauseGame);
            _pauseScreen.ExitClicked += ExitGame;
            _pauseScreen.RetryClicked += Restart;
            _pauseScreen.ResumeClicked += ContinueGame;
        }

        private void OnDisable()
        {
            _player.Touched -= OnTouched;

            _gameOverScreen.ExitClicked -= ExitGame;
            _gameOverScreen.RetryClicked -= Restart;

            _pauseButton.onClick.RemoveListener(PauseGame);
            _pauseScreen.ExitClicked -= ExitGame;
            _pauseScreen.RetryClicked -= Restart;
            _pauseScreen.ResumeClicked -= ContinueGame;
        }

        private void Restart()
        {
            _countdownScreen.Disable();
        }

        private void StartNewGame()
        {
            ResetValues();

            _squareSpawner.StartSpawn();
            _player.EnableInputDetection();
            StartTimerCoroutine();
        }

        private void ResetValues()
        {
            StopTimerCoroutine();
            _squareSpawner.StopSpawn();
            _squareSpawner.ReturnAllObjectsToPool();
            _player.DisableInputDetection();

            _timer = 0;
            _score = 0;
            _lives = 3;

            UpdateTimerText();
            UpdateUIText();
        }

        private void UpdateUIText()
        {
            _scoreText.text = _score.ToString();
            _livesText.text = _lives.ToString();
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

        private void OnTouched(InteractableObject @object)
        {
            Square interactableObject = (Square)@object;

            interactableObject.DisableMovement();

            if (interactableObject.CurrentType == SquareType.Empty)
            {
                interactableObject.EnableMinus10Sprite();

                if (GameLoader.IsTutorialMode)
                    return;

                UpdateScore(-10);
                _lives--;

                if (_lives > 0)
                {
                    UpdateUIText();
                    return;
                }

                EndGame();
            }
            else if (interactableObject.CurrentType == SquareType.Half)
            {
                interactableObject.EnablePlus10Sprite();
                UpdateScore(10);
            }
            else if (interactableObject.CurrentType == SquareType.Full)
            {
                interactableObject.EnablePlus100Sprite();
                UpdateScore(100);
            }
        }

        private void PauseGame()
        {
            _squareSpawner.StopSpawn();
            _squareSpawner.ReturnAllObjectsToPool();
            StopTimerCoroutine();
            _player.DisableInputDetection();

            _pauseScreen.EnableScreen(_score, _timerText.text, _lives);
        }

        private void ContinueGame()
        {
            _squareSpawner.StartSpawn();
            StartTimerCoroutine();
            _player.EnableInputDetection();
        }

        private void EndGame()
        {
            _squareSpawner.StopSpawn();
            _squareSpawner.ReturnAllObjectsToPool();
            StopTimerCoroutine();
            _player.DisableInputDetection();

            _gameOverScreen.EnableScreen(_score, _timerText.text, 0);

            if (!GameLoader.IsTutorialMode)
                RecordHolder.AddNewRecord(_gameType, _score, _timer);
        }

        private void UpdateScore(int scoreDelta)
        {
            _score += scoreDelta;

            if (_score < 0)
                _score = 0;

            _scoreText.text = $"{_score}";
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