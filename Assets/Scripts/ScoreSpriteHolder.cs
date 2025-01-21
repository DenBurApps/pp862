using System;
using System.Collections;
using UnityEngine;

public class ScoreSpriteHolder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _minus10, _plus10, _plus100;

    public event Action ReadyForDisable;

    private void OnEnable()
    {
        _minus10.gameObject.SetActive(false);
        _plus10.gameObject.SetActive(false);
        _plus100.gameObject.SetActive(false);
    }

    public void DisplayMinus10AndDisable()
    {
        StartCoroutine(DisplaySpriteAndDisable(_minus10));
    }
        
    public void DisplayPlus10AndDisable()
    {
        StartCoroutine(DisplaySpriteAndDisable(_plus10));
    }
        
    public void DisplayPlus100AndDisable()
    {
        StartCoroutine(DisplaySpriteAndDisable(_plus100));
    }

    private IEnumerator DisplaySpriteAndDisable(SpriteRenderer sprite)
    {
        sprite.gameObject.SetActive(true);
            
        yield return new WaitForSeconds(1f);
            
        sprite.gameObject.SetActive(false);
        ReadyForDisable?.Invoke();
    }
}