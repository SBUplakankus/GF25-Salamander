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
        private const int MovementSpeed = 35;
        private const Ease MovementEase = Ease.Linear;
        private const int RotationSpeed = 2;

        private void Start()
        {
            transform.position = startPosition.position;
            _currentTarget = startPosition;
            PickNextPosition();
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
            Tween.LocalPosition(transform, _currentTarget.position, MovementSpeed, MovementEase).OnComplete(PickNextPosition);
        }
    }
}
