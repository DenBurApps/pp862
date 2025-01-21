using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassicGame
{
    public class CircleSpawner : ObjectPool<Circle>
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Circle _prefab;
        [SerializeField] private SpawnArea _spawnArea;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private float _objMovingSpeed;
        [SerializeField] private Transform _targetPosition;

        private List<Circle> _spawnedObjects = new List<Circle>();
        private IEnumerator _spawnCoroutine;

        private void Awake()
        {
            for (int i = 0; i <= _poolCapacity; i++)
            {
                Initalize(_prefab);
            }
        }

        public void StartSpawn()
        {
            if (_spawnCoroutine != null) return;
            _spawnCoroutine = StartSpawning();
            StartCoroutine(_spawnCoroutine);
        }

        public void StopSpawn()
        {
            if (_spawnCoroutine == null) return;

            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator StartSpawning()
        {
            WaitForSeconds interval = new WaitForSeconds(_spawnInterval);

            while (true)
            {
                Spawn();
                yield return interval;
            }
        }

        private void Spawn()
        {
            if (ActiveObjects.Count >= _poolCapacity)
                return;
            

            if (TryGetObject(out Circle circle, _prefab))
            {
                circle.transform.position = _spawnArea.GetRandomPositionAroundScreen();
                _spawnedObjects.Add(circle);
                circle.EnableMovement(_targetPosition.position, _objMovingSpeed);
                circle.ReadyToDisable += ReturnToPool;
            }
        }

        public void SetMovingSpeed(int value)
        {
            _objMovingSpeed = value;
        }

        public void ReturnToPool(Circle circle)
        {
            if (circle == null)
                return;

            circle.StopMovement();
            circle.ReadyToDisable -= ReturnToPool;
            PutObject(circle);

            if (_spawnedObjects.Contains(circle))
                _spawnedObjects.Remove(circle);
        }
        
        public void ReturnAllObjectsToPool()
        {
            if (_spawnedObjects.Count <= 0)
                return;

            List<Circle> objectsToReturn = new List<Circle>(_spawnedObjects);
            foreach (var @object in objectsToReturn)
            {
                @object.StopMovement();
                ReturnToPool(@object);
            }
        }

    }
}