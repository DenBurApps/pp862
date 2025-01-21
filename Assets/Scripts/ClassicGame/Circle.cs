using System;
using UnityEngine;

namespace ClassicGame
{
    public class Circle : InteractableObject
    {
        [SerializeField] private CircleMover _circleMover;
        [SerializeField] private ScoreSpriteHolder _scoreSpriteHolder;

        public event Action<Circle> ReadyToDisable;

        private void OnEnable()
        {
            _circleMover.ReadyForDisable += OnReadyToDisable;
            _scoreSpriteHolder.ReadyForDisable += OnReadyToDisable;
        }

        private void OnDisable()
        {
            _circleMover.ReadyForDisable -= OnReadyToDisable;
            _scoreSpriteHolder.ReadyForDisable -= OnReadyToDisable;
        }

        public void EnableMovement(Vector3 target, float speed)
        {
            _circleMover.StartMovingTowards(target, speed);
        }

        public void StopMovement()
        {
            _circleMover.StopMoving();
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
    }
}
