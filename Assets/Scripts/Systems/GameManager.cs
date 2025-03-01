using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            _gameTime = 0;
        }

        private void Update()
        {
            _gameTime += Time.deltaTime;
            if (!(_gameTime >= GameLength)) return;
            
            OnTimerExpiration?.Invoke();
        }
    }
}
