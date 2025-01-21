using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableObjectLayer;
        
    public event Action<InteractableObject> Touched;

    private Coroutine _inputDetectionCoroutine;
    private InteractableObject _lastTouchedObject;

    public void EnableInputDetection()
    {
        if (_inputDetectionCoroutine == null)
        {
            _inputDetectionCoroutine = StartCoroutine(DetectInput());
        }
    }

    public void DisableInputDetection()
    {
        if (_inputDetectionCoroutine != null)
        {
            StopCoroutine(_inputDetectionCoroutine);
            _inputDetectionCoroutine = null;
        }
    }

    private IEnumerator DetectInput()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                HandleTouch(touch.position);
            }

            yield return null;
        }
    }

    private void HandleTouch(Vector3 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, _interactableObjectLayer);
        if (hit.collider != null)
        {
            InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();
            if (interactableObject != null && interactableObject != _lastTouchedObject)
            {
                _lastTouchedObject = interactableObject;
                Touched?.Invoke(interactableObject);
            }
        }
    }
}