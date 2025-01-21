using System;
using UnityEngine;
using RecordSystem;
using TMPro;
using UnityEngine.UI;

namespace MainScreen
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class RecordsScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _totalScoreText;
        [SerializeField] private TMP_Text _classicTime, _classicScore;
        [SerializeField] private TMP_Text _pianoTime, _pianoScore;
        [SerializeField] private TMP_Text _sequenceTime, _sequenceScore;
        [SerializeField] private Button _closeButton;
        
        private ScreenVisabilityHandler _screenVisabilityHandler;

        public event Action ScreenClosed;
        
        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseClicked);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        private void Start()
        {
            _screenVisabilityHandler.DisableScreen();
        }

        public void EnableScreen()
        {
            _screenVisabilityHandler.EnableScreen();

            var classicRecord = RecordHolder.GetRecordByType(GameType.Classic);
            var pianoRecord = RecordHolder.GetRecordByType(GameType.Piano);
            var sequenceRecord = RecordHolder.GetRecordByType(GameType.Sequence);

            _totalScoreText.text = RecordHolder.GetTotalScores().ToString();
            
            _classicTime.text = GetTimeText(classicRecord.TotalTime);
            _classicScore.text = classicRecord.TotalScore.ToString();
            
            _pianoTime.text = GetTimeText(pianoRecord.TotalTime);
            _pianoScore.text = pianoRecord.TotalScore.ToString();
            
            _sequenceTime.text = GetTimeText(sequenceRecord.TotalTime);
            _sequenceScore.text = sequenceRecord.TotalScore.ToString();
        }

        private string GetTimeText(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            return $"{minutes:00}:{seconds:00}";
        }

        private void OnCloseClicked()
        {
            ScreenClosed?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }
}
