    using System;
    using AI;
    using Player;
    using UnityEngine;
using World;

namespace Systems
{
    public struct FinalStats
    {
        public int FoodScore;
        public int MoistScore;
        public int EnviroDamage;
        public int EnemyDamage;
        public int EnemiesDefeated;
        public float TimeSurvived;
        public int TimeScore;
        public int FinalScore;
    }
    public class StatsTracker : MonoBehaviour
    {
        
        [Header("Tracked Stats")] 
        private int _foodScore = 0;
        private int _moistScore = 0;
        private int _environmentDamageTaken = 0;
        private int _enemyDamageTaken = 0;
        private int _enemiesDefeated = 0;
        private float _timeSurvived = 0;
        private int _timeScore = 0;

        [Header("Game Checks")] 
        private bool _gameStarted = false;

        public static event Action<FinalStats> OnGameEnd;

        private void OnEnable()
        {
            TutorialController.OnTutorialEnd += HandleTutorialEnd;
            FoodObject.OnFoodPickup += HandleFood;
            DamageObject.OnPlayerDamage += HandleEnvironmentDamage;
            DamageObjectAOE.OnDamagePlayer += HandleEnvironmentDamage;
            PlayerAttributes.OnMoistEnter += HandleMoist;
            EnemyController.OnPlayerDamage += HandleEnemyDamage;
            EnemyController.OnEnemyRetreat += HandleEnemyDefeated;
            PlayerAttributes.OnGameOver += HandleEndOfGame;
        }

        private void OnDisable()
        {
            TutorialController.OnTutorialEnd -= HandleTutorialEnd;
            FoodObject.OnFoodPickup -= HandleFood;
            DamageObject.OnPlayerDamage -= HandleEnvironmentDamage;
            DamageObjectAOE.OnDamagePlayer -= HandleEnvironmentDamage;
            PlayerAttributes.OnMoistEnter -= HandleMoist;
            EnemyController.OnPlayerDamage -= HandleEnemyDamage;
            EnemyController.OnEnemyRetreat -= HandleEnemyDefeated;
            PlayerAttributes.OnGameOver -= HandleEndOfGame;
        }

        private void Update()
        {
            if (!_gameStarted) return;
            _timeSurvived += Time.deltaTime;
        }

        private void HandleTutorialEnd()
        {
            _gameStarted = true;
        }

        private void HandleMoist(int amount)
        {
            _moistScore += amount;
        }

        private void HandleFood(int amount)
        {
            _foodScore += amount;
        }

        private void HandleEnvironmentDamage(int amount)
        {
            _environmentDamageTaken += amount;
        }

        private void HandleEnemyDamage(int amount)
        {
            _enemyDamageTaken += amount;
        }

        private void HandleEnemyDefeated()
        {
            _enemiesDefeated++;
        }

        private void HandleEndOfGame()
        {
            Debug.Log("Bosh");
            CalculateScore();
            var final = new FinalStats
            {
                FoodScore = _foodScore,
                MoistScore = _moistScore,
                EnviroDamage = _environmentDamageTaken,
                EnemyDamage = _enemyDamageTaken,
                EnemiesDefeated = _enemiesDefeated,
                TimeScore = _timeScore,
                TimeSurvived = _timeSurvived,
                FinalScore = 2000
            };
            OnGameEnd?.Invoke(final);
        }

        private void CalculateScore()
        {
            _foodScore *= 10;
            _moistScore *= 5;
            _environmentDamageTaken *= -2;
            _enemyDamageTaken *= -2;
            _enemiesDefeated *= 1000;
            _timeScore = (int)_timeSurvived * 100;
        }
    }
}
