using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class TraningScreen : MonoBehaviour
{
    [SerializeField] private Button _closeButton, _classicButton, _pianoButton, _sequenceButton;
    [SerializeField] private Button _classicPlayButton, _pianoPlayButton, _sequencePlayButton;
    [SerializeField] private DifficultySelectionScreen _classic, _piano, _sequence;

    public event Action CloseClicked;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnEnable()
    {
        _classic.Closed += _screenVisabilityHandler.EnableScreen;
        _piano.Closed += _screenVisabilityHandler.EnableScreen;
        _sequence.Closed += _screenVisabilityHandler.EnableScreen;

        _closeButton.onClick.AddListener(OnCloseClicked);

        _classicPlayButton.onClick.AddListener(OnClassicPlay);
        _pianoPlayButton.onClick.AddListener(OnPianoPlay);
        _sequencePlayButton.onClick.AddListener(OnSequencePlay);

        _classicButton.onClick.AddListener(EnableClassic);
        _pianoButton.onClick.AddListener(EnablePiano);
        _sequenceButton.onClick.AddListener(EnableSequence);
    }

    private void OnDisable()
    {
        _classic.Closed -= _screenVisabilityHandler.EnableScreen;
        _piano.Closed -= _screenVisabilityHandler.EnableScreen;
        _sequence.Closed -= _screenVisabilityHandler.EnableScreen;

        _closeButton.onClick.RemoveListener(OnCloseClicked);

        _classicPlayButton.onClick.RemoveListener(OnClassicPlay);
        _pianoPlayButton.onClick.RemoveListener(OnPianoPlay);
        _sequencePlayButton.onClick.RemoveListener(OnSequencePlay);

        _classicButton.onClick.RemoveListener(EnableClassic);
        _pianoButton.onClick.RemoveListener(EnablePiano);
        _sequenceButton.onClick.RemoveListener(EnableSequence);
    }

    public void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
    }
    
    private void OnCloseClicked()
    {
        CloseClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnClassicPlay()
    {
        int selectedDifficulty = Mathf.RoundToInt(_classic.Slider.value);

        GameLoader.SetGameSettings(selectedDifficulty, true);
        SceneManager.LoadScene("ClassicModeScene");
    }

    private void OnPianoPlay()
    {
        int selectedDifficulty = Mathf.RoundToInt(_piano.Slider.value);

        GameLoader.SetGameSettings(selectedDifficulty, true);
        SceneManager.LoadScene("PianoModeScene");
    }

    private void OnSequencePlay()
    {
        int selectedDifficulty = Mathf.RoundToInt(_piano.Slider.value);

        GameLoader.SetGameSettings(selectedDifficulty, true);
        SceneManager.LoadScene("SequenceModeScene");
    }

    private void EnableClassic()
    {
        _classic.EnableScreen();
        _screenVisabilityHandler.DisableScreen();
    }

    private void EnablePiano()
    {
        _piano.EnableScreen();
        _screenVisabilityHandler.DisableScreen();
    }

    private void EnableSequence()
    {
        _sequence.EnableScreen();
        _screenVisabilityHandler.DisableScreen();
    }
}