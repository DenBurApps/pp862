using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SequenceGame
{
    public class StarHolder : MonoBehaviour
    {
        [SerializeField] private Star[] _stars;

        private int _sequenceCount;
        private float _sequenceInterval = 1.5f;
        private List<Star> _shownElements = new();
        private IEnumerator _disablingCoroutine;
        private bool _isPaused;

        public event Action AllElementsShown;
        public event Action ElementCorrectlyChosen;
        public event Action ElementIncorrectlyChosen;
        public event Action AllElementsCorrectlyChosen;

        private void OnEnable()
        {
            foreach (var element in _stars)
            {
                element.ElementClicked += OnElementClicked;
            }
        }

        private void OnDisable()
        {
            foreach (var element in _stars)
            {
                element.ElementClicked -= OnElementClicked;
            }
        }

        public void DisableAllElements()
        {
            foreach (var element in _stars)
            {
                element.gameObject.SetActive(false);
            }
        }

        public void EnableAllElements()
        {
            foreach (var element in _stars)
            {
                element.gameObject.SetActive(true);
            }
        }

        public void ActivateAllElements()
        {
            foreach (var element in _stars)
            {
                element.Enable();
            }
        }

        public void DiactivateAllElements()
        {
            foreach (var element in _stars)
            {
                element.Disable();
            }
        }
        
        public void StartSequence()
        {
            StopSequence();

            _shownElements.Clear();
            _disablingCoroutine = SequenceCoroutine();
            DiactivateAllElements();
            StartCoroutine(_disablingCoroutine);
        }

        public void StopSequence()
        {
            if (_disablingCoroutine == null) return;

            StopCoroutine(_disablingCoroutine);
            _disablingCoroutine = null;
        }

        public void SetSequenceCount(int count)
        {
            if (count <= 0 || _stars == null || count > _stars.Length)
                return;

            _sequenceCount = count;
        }

        public void PauseSequence()
        {
            _isPaused = true;
        }

        public void ResumeSequence()
        {
            _isPaused = false;
        }

        private IEnumerator SequenceCoroutine()
        {
            var interval = new WaitForSeconds(_sequenceInterval);

            for (int i = 0; i < _sequenceCount; i++)
            {
                while (_isPaused)
                {
                    yield return null;
                }

                yield return interval;

                var randomIndex = Random.Range(0, _stars.Length);
                _stars[randomIndex].gameObject.SetActive(true);
                _stars[randomIndex].StartDisabling();
                _shownElements.Add(_stars[randomIndex]);

                yield return interval;
            }

            AllElementsShown?.Invoke();
        }

        private void OnElementClicked(Star clickedElement)
        {
            if (_shownElements.Count == 0) return;

            bool isCorrect = _shownElements[0] == clickedElement;

            if (isCorrect)
            {
                HandleCorrectChoice(clickedElement);
            }
            else
            {
                HandleIncorrectChoice(clickedElement);
            }
        }
        
        private void HandleCorrectChoice(Star clickedElement)
        {
            clickedElement.EnablePlus100Sprite();
            _shownElements.RemoveAt(0);
            ElementCorrectlyChosen?.Invoke();
            
            if (_shownElements.Count == 0)
            {
                AllElementsCorrectlyChosen?.Invoke();
            }
        }
        
        private void HandleIncorrectChoice(Star clickedElement)
        {
            clickedElement.EnableMinus10Sprite();
            ElementIncorrectlyChosen?.Invoke();
        }
    }
}
