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
        [SerializeField] private List<Transform> movePositions;
        private Transform _currentTarget;
        private const int MovementSpeed = 1;
        private const int RotationSpeed = 2;

        private void Start()
        {
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
            if (movePositions.Count == 0) return;
            var newTarget = _currentTarget;
            
            while (newTarget == _currentTarget )
            {
                var rng = Random.Range(0, movePositions.Count);
                newTarget = movePositions[rng];
            }
    
            _currentTarget = newTarget;
            LookTowardsNextPosition();
        }

        private void MoveToNextPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, MovementSpeed * Time.deltaTime);
        }

        private void LookTowardsNextPosition()
        {
            transform.LookAt(_currentTarget);
        }
    }
}
