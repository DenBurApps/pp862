using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PianoModeGame
{
    public class Square : InteractableObject
    {
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private Sprite _halfSprite;
        [SerializeField] private Sprite _fullSprite;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _yCenterPosition;
        [SerializeField] private ScoreSpriteHolder _scoreSpriteHolder;

        private float _speed = 0;
        private SquareType _currentType;
        private Transform _transform;
        private IEnumerator _movingCoroutine;

        public event Action<Square> ReadyToDisable;
        
        public SquareType CurrentType => _currentType;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            DetermineRandomType();
            UpdateSprite();

            _scoreSpriteHolder.ReadyForDisable += OnReadyToDisable;
        }

        private void OnDisable()
        {
            _scoreSpriteHolder.ReadyForDisable -= OnReadyToDisable;
        }

        public void EnableMovement()
        {
            if (_movingCoroutine == null)
            {
                _movingCoroutine = StartMoving();
                StartCoroutine(_movingCoroutine);
            }
        }

        public void DisableMovement()
        {
            if (_movingCoroutine != null)
            {
                StopCoroutine(_movingCoroutine);
                _movingCoroutine = null;
            }
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void EnableMinus10Sprite()
        {
            _scoreSpriteHolder.DisplayMinus10AndDisable();
        }

        public void EnablePlus10Sprite()
        {
            _scoreSpriteHolder.DisplayPlus10AndDisable();
        }

        public void EnablePlus100Sprite()
        {
            _scoreSpriteHolder.DisplayPlus100AndDisable();
        }
        
        private void OnReadyToDisable()
        {
            ReadyToDisable?.Invoke(this);
        }
        
        private IEnumerator StartMoving()
        {
            Vector3 movingDirection = _transform.position.y > _yCenterPosition ? Vector3.down : Vector3.up;
            
            while (true)
            {
                _transform.position += movingDirection * (_speed * Time.deltaTime);

                yield return null;
            }
        }

        private void DetermineRandomType()
        {
            int randomValue = Random.Range(0, 3);
            _currentType = (SquareType)randomValue;
        }

        private void UpdateSprite()
        {
            switch (_currentType)
            {
                case SquareType.Empty:
                    _spriteRenderer.sprite = _emptySprite;
                    break;
                case SquareType.Half:
                    _spriteRenderer.sprite = _halfSprite;
                    break;
                case SquareType.Full:
                    _spriteRenderer.sprite = _fullSprite;
                    break;
            }
        }
    }

    public enum SquareType
    {
        Empty,
        Half,
        Full
    }
}