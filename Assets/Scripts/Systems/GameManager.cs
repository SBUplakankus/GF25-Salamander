using System;
using System.Collections;
using UnityEngine;

namespace Systems
{
    public class GameManager : MonoBehaviour
    {
        [Header("Event Params")]
        [SerializeField] private int eventInterval = 30;
        
        private bool _mainGameStarted;
        private bool _eventReady;

        private void Start()
        {
            _mainGameStarted = false;
            _eventReady = false;
        }

        private void Update()
        {
            if (!_mainGameStarted || !_eventReady) return;
            StartCoroutine(EventCoroutine());
        }

        private void HandleGameStart()
        {
            _mainGameStarted = true;
        }

        private IEnumerator EventCoroutine()
        {
            _eventReady = false;
            // TODO: Event Logic - Switch Statement with an Index maybe that triggers garbage, spills, trees, pollution etc...
            yield return new WaitForSeconds(eventInterval);
            _eventReady = true;
        }
    }
}
