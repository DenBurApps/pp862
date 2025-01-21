using System;
using System.Collections;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ClassicGame
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _livesText;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Image _sparklesImage;
        [SerializeField] private CountdownScreen _countdownScreen;
        [SerializeField] private Transform _centerPosition;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private GameOverScreen _gameOverScreen;
        [SerializeField] private Player _player;
        [SerializeField] private CircleSpawner _circleSpawner;
        [SerializeField] private TutorialScreen _tutorialScreen;
        [SerializeField] private GameType _gameType;
        [SerializeField] private GameObject _disableObject;

        private int _score;
        private float _timer;
        private int _lives;
        private IEnumerator _timerCoroutine;

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

        private void Start()
        {
            _sparklesImage.gameObject.SetActive(false);

            if (GameLoader.IsTutorialMode)
            {
                int difficulty = GameLoader.SelectedDifficulty;
                _circleSpawner.SetMovingSpeed(difficulty);
                _tutorialScreen.gameObject.SetActive(false);
                _countdownScreen.EnableScreen();
                _countdownScreen.Disable();
            }
        }

        private void Restart()
        {
            _countdownScreen.Disable();
        }

        private void StartNewGame()
        {
            ResetValues();

            _circleSpawner.StartSpawn();
            _player.EnableInputDetection();
            StartTimerCoroutine();
        }

        private void ResetValues()
        {
            StopTimerCoroutine();
            _circleSpawner.StopSpawn();
            _circleSpawner.ReturnAllObjectsToPool();
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

        private void OnTouched(InteractableObject circle)
        {
            Circle interactableObject = (Circle)circle;

            float distance = Vector3.Distance(circle.transform.position, _centerPosition.position);
            interactableObject.StopMovement();

            if (distance > 1.3f)
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
            else if (distance > 0.3f && distance <= 1.3f)
            {
                interactableObject.EnablePlus10Sprite();
                UpdateScore(10);
            }
            else if (distance <= 0.3f)
            {
                interactableObject.EnablePlus100Sprite();
                UpdateScore(100);

                if (_sparklesImage.IsActive())
                    _sparklesImage.gameObject.SetActive(false);

                _sparklesImage.gameObject.SetActive(true);
            }
        }

        private void PauseGame()
        {
            _circleSpawner.StopSpawn();
            _circleSpawner.ReturnAllObjectsToPool();
            StopTimerCoroutine();
            _player.DisableInputDetection();

            _pauseScreen.EnableScreen(_score, _timerText.text, _lives);
        }

        private void ContinueGame()
        {
            _circleSpawner.StartSpawn();
            StartTimerCoroutine();
            _player.EnableInputDetection();
        }

        private void EndGame()
        {
            _circleSpawner.StopSpawn();
            _circleSpawner.ReturnAllObjectsToPool();
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