using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoModeGame
{
    public class SquareSpawner : ObjectPool<Square>
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Square _prefab;
        [SerializeField] private Transform[] _spawnAreas;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private float _objMovingSpeed;
        [SerializeField] private int _objectsPerSpawn = 1;

        private int _lastSpawnIndex = -1;
        private List<Square> _spawnedObjects = new List<Square>();
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

            List<int> usedIndices = new List<int>();

            for (int i = 0; i < _objectsPerSpawn; i++)
            {
                if (TryGetObject(out Square square, _prefab))
                {
                    int randomIndex;

                    do
                    {
                        randomIndex = Random.Range(0, _spawnAreas.Length);
                    } while (usedIndices.Contains(randomIndex) || randomIndex == _lastSpawnIndex);

                    usedIndices.Add(randomIndex);
                    _lastSpawnIndex = randomIndex;

                    square.transform.position = _spawnAreas[randomIndex].position;
                    square.SetSpeed(_objMovingSpeed);
                    _spawnedObjects.Add(square);
                    square.EnableMovement();
                    square.ReadyToDisable += ReturnToPool;
                }
            }
        }


        public void SetMovingSpeed(int value)
        {
            _objMovingSpeed = value;
        }

        public void ReturnToPool(Square Square)
        {
            if (Square == null)
                return;

            Square.DisableMovement();
            Square.ReadyToDisable -= ReturnToPool;
            PutObject(Square);

            if (_spawnedObjects.Contains(Square))
                _spawnedObjects.Remove(Square);
        }

        public void ReturnAllObjectsToPool()
        {
            if (_spawnedObjects.Count <= 0)
                return;

            List<Square> objectsToReturn = new List<Square>(_spawnedObjects);
            foreach (var @object in objectsToReturn)
            {
                @object.DisableMovement();
                ReturnToPool(@object);
            }
        }
    }
}