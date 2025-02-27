using System;
using System.Collections;
using UnityEngine;

namespace Systems
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Params")]
        private const int GameLength = 150;
        private float _gameTime;

        public static event Action OnTimerExpiration;
        

        private void Start()
        {
            Application.targetFrameRate = 60;
            _gameTime = 0;
        }

        private void Update()
        {
            _gameTime += Time.deltaTime;
            if (_gameTime >= GameLength)
            {
                OnTimerExpiration?.Invoke();
            }
        }
    }
}
