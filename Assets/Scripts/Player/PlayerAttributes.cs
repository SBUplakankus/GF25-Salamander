using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerAttributes : MonoBehaviour
    {
        [Header("Player Attributes")]
        [SerializeField] private int healthLevel;
        [SerializeField] private int moistLevel;
        [SerializeField] private int hungerLevel;

        [Header("Moist Level Params")] 
        [SerializeField] private const int MoistReductionInterval = 1;
        [SerializeField] private const int MoistRegenAmount = 5;
        [SerializeField] private const int MoistReductionAmount = 5;
        [SerializeField] private const int ZeroMoistDamage = 5;
        
        [Header("Hunger Level Params")] 
        [SerializeField] private const int HungerReductionInterval = 1;
        [SerializeField] private const int HungerReductionAmount = 5;
        [SerializeField] private const int ZeroHungerDamage = 5;
        
        [Header("Health Level Params")] 
        [SerializeField] private const int HealthRegenInterval = 1;
        [SerializeField] private const int HealthRegenAmount = 5;
        
        private int _maxHealth;
        private int _maxMoist;
        private int _maxHunger;
        private bool _isMoist = false;
        private bool _moistModifierReady = true;
        private bool _hungerModifierReady = true;
        private bool _healthModifierReady = true;
        
        #region Events
        public static event Action<int> OnHealthLevelChanged;
        public static event Action<int> OnMoistLevelChanged;
        public static event Action<int> OnHungerLevelChanged;
        #endregion
        
        #region Game Functions
        private void Awake()
        {
            SetInitialLimits();
        }

        private void Update()
        {
            if (_moistModifierReady)
            {
                if (_isMoist)
                {
                    StartCoroutine(MoistCoroutine(true));
                }
                else
                {
                    StartCoroutine(moistLevel > 0 ? MoistCoroutine(false) : ZeroMoistDamageCoroutine());
                }
            }

            if (_hungerModifierReady)
            {
                StartCoroutine(hungerLevel > 0 ? HungerCoroutine() : ZeroHungerDamageCoroutine());
            }

            if (healthLevel >= _maxHealth) return;

            if (_healthModifierReady)
            {
                StartCoroutine(HealthRegenCoroutine());
            }
        }
        #endregion
        
        #region Attribute Modifiers
        // Decrease Attributes
        private void DecreaseMoistLevel(int amount)
        {
            moistLevel -= amount;
            if (moistLevel <= 0)
            {
                // TODO: Event
            }
            OnMoistLevelChanged?.Invoke(moistLevel);
        }
        
        private void DecreaseHungerLevel(int amount)
        {
            hungerLevel -= amount;
            if (hungerLevel <= 0)
            {
                // TODO: Event
            }
            OnHungerLevelChanged?.Invoke(hungerLevel);
        }
        
        public void DecreaseHealthLevel(int amount)
        {
            healthLevel -= amount;
            if (healthLevel <= 0)
            {
                // TODO: Event
            }
            OnHealthLevelChanged?.Invoke(healthLevel);
        }
        
        // Increase Attributes
        private void IncreaseMoistLevel(int amount)
        {
            var temp = moistLevel += amount;
            moistLevel = temp >= _maxMoist ? _maxMoist : temp;
            OnMoistLevelChanged?.Invoke(moistLevel);
        }

        private void IncreaseHealthLevel(int amount)
        {
            var temp = healthLevel += amount;
            healthLevel = temp >= _maxHealth ? _maxHealth : temp;
            OnHealthLevelChanged?.Invoke(healthLevel);
        }
        
        public void IncreaseHungerLevel(int amount)
        {
            var temp = hungerLevel += amount;
            hungerLevel = temp >= _maxHunger ? _maxHunger : temp;
            OnHungerLevelChanged?.Invoke(hungerLevel);
        }
        
        // Initialise Attributes
        private void SetInitialLimits()
        {
            _maxMoist = moistLevel;
            _maxHunger = hungerLevel;
            _maxHealth = healthLevel;
        }
        #endregion
        
        #region Coroutines
        private IEnumerator MoistCoroutine(bool inWater)
        {
            if (inWater)
            {
                IncreaseMoistLevel(MoistRegenAmount);
            }
            else
            {
                DecreaseMoistLevel(MoistReductionAmount);
            }
            
            _moistModifierReady = false;
            yield return new WaitForSeconds(MoistReductionInterval);
            _moistModifierReady = true;
        }
        private IEnumerator ZeroMoistDamageCoroutine()
        {
            DecreaseHealthLevel(ZeroMoistDamage);
            _moistModifierReady = false;
            yield return new WaitForSeconds(MoistReductionInterval);
            _moistModifierReady = true;
        }
        
        private IEnumerator HungerCoroutine()
        {
            DecreaseHungerLevel(HungerReductionAmount);
            _hungerModifierReady = false;
            yield return new WaitForSeconds(HungerReductionInterval);
            _hungerModifierReady = true;
        }
        
        private IEnumerator ZeroHungerDamageCoroutine()
        {
            DecreaseHealthLevel(ZeroHungerDamage);
            _hungerModifierReady = false;
            yield return new WaitForSeconds(HungerReductionInterval);
            _hungerModifierReady = true;
        }
        
        private IEnumerator HealthRegenCoroutine()
        {
            IncreaseHealthLevel(HealthRegenAmount);
            _healthModifierReady = false;
            yield return new WaitForSeconds(HealthRegenInterval);
            _healthModifierReady = true;
        }
        #endregion
    }
}
