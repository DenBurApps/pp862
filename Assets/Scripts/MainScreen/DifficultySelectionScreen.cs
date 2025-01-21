using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class DifficultySelectionScreen : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _closeButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action Closed;
    public Slider Slider => _slider;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void Start()
    {
        DisableScreen();
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void DisableScreen()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnButtonClicked()
    {
        Closed?.Invoke();
        DisableScreen();
    }
}
