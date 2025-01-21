using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class TutorialScreen : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _playButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action PlayClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnButtonClicked);
        _playButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnButtonClicked);
        _playButton.onClick.RemoveListener(OnButtonClicked);
    }

    private void Start()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    private void OnButtonClicked()
    {
        PlayClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }
}