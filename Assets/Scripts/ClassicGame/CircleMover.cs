using System;
using System.Collections;
using UnityEngine;

namespace ClassicGame
{
    public class CircleMover : MonoBehaviour
    {
        private Coroutine _movementCoroutine;
        private Transform _transform;

        public event Action ReadyForDisable;
        
        private void Awake()
        {
            _transform = transform;
        }

        public void StartMovingTowards(Vector3 targetPosition, float speed)
        {
            StopMoving();
            _movementCoroutine = StartCoroutine(MoveTowardsTarget(targetPosition, speed));
        }

        public void StopMoving()
        {
            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
                _movementCoroutine = null;
            }
        }

        private IEnumerator MoveTowardsTarget(Vector3 targetPosition, float speed)
        {
            while (Vector3.Distance(_transform.position, targetPosition) > 0.01f)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            ReadyForDisable?.Invoke();
        }
    }
}