using System;
using UnityEngine;

namespace PianoModeGame
{
    public class PoolReturner : MonoBehaviour
    {
        [SerializeField] private SquareSpawner _squareSpawner;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Square square))
            {
                _squareSpawner.ReturnToPool(square);
            }
        }
    }
}
