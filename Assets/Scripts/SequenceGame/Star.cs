using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _highlightSprite;
    
    [SerializeField] private Image _image;
    [SerializeField] private Image _glowImage;
    [SerializeField] private float _disablingPeriod;
    [SerializeField] private Button _button;

    [SerializeField] private Image _minus10, _plus100;

    private IEnumerator _disablingCoroutine;
        
    public event Action<Star> ElementClicked;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnElementClicked);
        _image.sprite = _defaultSprite;
        _glowImage.gameObject.SetActive(false);
        _minus10.gameObject.SetActive(false);
        _plus100.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnElementClicked);
    }

    public void StartDisabling()
    {
        _button.enabled = false;
        StopDisabling();
            
        if (_disablingCoroutine != null) return; 

        _disablingCoroutine = DisablingCoroutine();
        StartCoroutine(_disablingCoroutine);
    }

    public void StopDisabling()
    {
        if(_disablingCoroutine == null) return;
            
        StopCoroutine(_disablingCoroutine);
        _disablingCoroutine = null;
    }
    
    public void EnableMinus10Sprite()
    {
        _minus10.gameObject.SetActive(true);
        
        StartCoroutine(HideImageAfterDelay(_minus10));
    }

    public void EnablePlus100Sprite()
    {
        StartDisabling();
        _plus100.gameObject.SetActive(true);
        StartCoroutine(HideImageAfterDelay(_plus100));
    }

    private IEnumerator HideImageAfterDelay(Image image)
    {
        yield return new WaitForSeconds(1f);
        image.gameObject.SetActive(false);
    }

    public void Enable()
    {
        _image.sprite = _defaultSprite;
        _glowImage.gameObject.SetActive(false);
        _button.enabled = true;
    }

    public void Disable()
    {
        _button.enabled = false;
    }
    
    private IEnumerator DisablingCoroutine()
    {
        _image.sprite = _highlightSprite;
        _glowImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(_disablingPeriod);
        
        _image.sprite = _defaultSprite;
        _glowImage.gameObject.SetActive(false);
    }
        
    private void OnElementClicked()
    {
        ElementClicked?.Invoke(this);
    }
}
