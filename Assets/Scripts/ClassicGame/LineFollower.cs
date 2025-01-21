using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassicGame
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineFollower : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private int _maxPositions = 3;

        private LineRenderer _lineRenderer;
        private float[] _positionLifetimes;
        private Coroutine _lineUpdateCoroutine;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            _lineRenderer.positionCount = 0;
            _positionLifetimes = new float[_maxPositions];
            
            StartLine();
        }
        
        public void StartLine()
        {
            if (_lineUpdateCoroutine == null)
            {
                _lineUpdateCoroutine = StartCoroutine(UpdateLine());
            }
        }

        public void StopLine()
        {
            if (_lineUpdateCoroutine != null)
            {
                StopCoroutine(_lineUpdateCoroutine);
                _lineUpdateCoroutine = null;
            }
        }

        private IEnumerator UpdateLine()
        {
            while (true)
            {
                AddPosition(_player.position);
                yield return null;
            }
        }

        private void AddPosition(Vector3 newPosition)
        {
            if (_lineRenderer.positionCount >= _maxPositions)
            {
                ShiftPositions();
            }

            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, newPosition);

            _positionLifetimes[_lineRenderer.positionCount - 1] = 1f;
        }

        private void ShiftPositions()
        {
            for (int i = 1; i < _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i - 1, _lineRenderer.GetPosition(i));
                _positionLifetimes[i - 1] = _positionLifetimes[i];
            }

            _lineRenderer.positionCount--;
        }
    }
}
