using System;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainScreen
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _totalScoreText;
        [SerializeField] private Button _recordsButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _playClassicButton;
        [SerializeField] private Button _playPianoButton;
        [SerializeField] private Button _playSequenceButton;
        [SerializeField] private Button _playTrainingButton;
        [SerializeField] private RecordsScreen _recordsScreen;
        [SerializeField] private TraningScreen _traningScreen;
        [SerializeField] private Settings _settings;
            
        private ScreenVisabilityHandler _screenVisabilityHandler;
        
        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            _recordsButton.onClick.AddListener(OpenRecords);
            _playTrainingButton.onClick.AddListener(OpenTraining);
            _settingsButton.onClick.AddListener(OpenSettings);
            
            _playClassicButton.onClick.AddListener(OpenClassicGame);
            _playPianoButton.onClick.AddListener(OpenPianoGame);
            _playSequenceButton.onClick.AddListener(OpenSequenceGame);
        }

        private void OnDisable()
        {
            _recordsButton.onClick.RemoveListener(OpenRecords);
            _playTrainingButton.onClick.RemoveListener(OpenTraining);
            _settingsButton.onClick.RemoveListener(OpenSettings);
            
            _playClassicButton.onClick.RemoveListener(OpenClassicGame);
            _playPianoButton.onClick.RemoveListener(OpenPianoGame);
            _playSequenceButton.onClick.RemoveListener(OpenSequenceGame);
        }

        private void Start()
        {
            _screenVisabilityHandler.EnableScreen();
            _totalScoreText.text = RecordHolder.GetTotalScores().ToString();
        }

        private void OpenRecords()
        {
            _recordsScreen.EnableScreen();
        }

        private void OpenTraining()
        {
            _traningScreen.EnableScreen();
        }

        private void OpenSettings()
        {
            _settings.ShowSettings();
        }

        private void OpenClassicGame()
        {
            SceneManager.LoadScene("ClassicModeScene");
        }

        private void OpenPianoGame()
        {
            SceneManager.LoadScene("PianoModeScene");
        }

        private void OpenSequenceGame()
        {
            SceneManager.LoadScene("SequenceModeScene");
        }
    }
}
