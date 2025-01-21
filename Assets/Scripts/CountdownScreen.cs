using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class CountdownScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private TutorialScreen _tutorialScreen;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action Disabled;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        if (_tutorialScreen != null)
            _tutorialScreen.PlayClicked += Disable;
        
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnDisable()
    {
        if (_tutorialScreen != null)
            _tutorialScreen.PlayClicked -= Disable;
    }

    public void Disable()
    {
        StartCoroutine(DisablingCoroutine());
    }

    public void EnableScreen()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void DisableScreen()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private IEnumerator DisablingCoroutine()
    {
        _screenVisabilityHandler.EnableScreen();
        var interval = new WaitForSeconds(1);

        for (int i = 3; i > 0; i--)
        {
            _countdownText.text = i.ToString();

            yield return interval;
        }

        Disabled?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }
}