using System;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class Firefly : MonoBehaviour
    {
        [Header("Firefly Positions")]
        [SerializeField] private Transform startPosition;
        [SerializeField] private List<Transform> movePositions;
        private Transform _currentTarget;
        private const int MovementSpeed = 5;

        private void Start()
        {
            transform.position = startPosition.position;
            _currentTarget = startPosition;
            PickNextPosition();
        }

        private void Update()
        {
            var remainingDistance = Vector3.Distance(transform.position, _currentTarget.position);
            if (remainingDistance <= 1)
                PickNextPosition();
            else
                MoveToNextPosition();
            
        }

        private void PickNextPosition()
        {
            if (!_currentTarget || movePositions.Count == 0) return;
            var newTarget = _currentTarget;
            
            while (newTarget == _currentTarget )
            {
                var rng = Random.Range(0, movePositions.Count);
                newTarget = movePositions[rng];
            }

            _currentTarget = newTarget;
        }

        private void MoveToNextPosition()
        {
            Vector3.MoveTowards(transform.position, _currentTarget.position, MovementSpeed * Time.deltaTime);
        }
    }
}
