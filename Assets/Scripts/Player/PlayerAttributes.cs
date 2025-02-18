using System;
using System.Collections;
using AI;
using UnityEngine;
using World;

namespace Player
{
    public class PlayerAttributes : MonoBehaviour
    {
        [Header("Player Attributes")]
        [SerializeField] private int healthLevel;
        [SerializeField] private int moistLevel;
        [SerializeField] private int hungerLevel;

        [Header("Moist Level Params")] 
        [SerializeField] private int moistReductionInterval = 1;
        [SerializeField] private int moistReductionAmount = 5;
        [SerializeField] private int moistRegenInterval = 1;
        [SerializeField] private int moistRegenAmount = 5;
        [SerializeField] private int zeroMoistDamage = 5;
        
        [Header("Hunger Level Params")] 
        [SerializeField] private int hungerReductionInterval = 1;
        [SerializeField] private int hungerReductionAmount = 5;
        [SerializeField] private int zeroHungerDamage = 5;
        
        [Header("Health Level Params")] 
        [SerializeField] private int healthRegenInterval = 1;
        [SerializeField] private int healthRegenAmount = 5;
        
        private int _maxHealth;
        private int _maxMoist;
        private int _maxHunger;
        private bool _isMoist;
        private bool _takingDamage;
        private bool _moistModifierReady = true;
        private bool _hungerModifierReady = true;
        private bool _healthModifierReady = true;
        
        #region Events
        public static event Action<int> OnHealthLevelChanged;
        public static event Action<int> OnMoistLevelChanged;
        public static event Action<int> OnHungerLevelChanged;
        public static event Action OnDamageTaken;
        public static event Action OnGameOver;
        public static event Action<int, int, int> OnInitSliderValues; 
        #endregion
        
        #region Unity Functions
        private void Awake()
        {
            SetInitialLimits();
        }

        private void Start()
        {
            OnInitSliderValues?.Invoke(healthLevel, moistLevel, hungerLevel);
        }

        private void OnEnable()
        {
            DamageObject.OnPlayerDamage += DecreaseHealthLevel;
            DamageObjectAOE.OnDamagePlayer += DecreaseHealthLevel;
            FoodObject.OnFoodPickup += IncreaseHungerLevel;
            EnemyController.OnPlayerDamage += DecreaseHealthLevel;
        }

        private void OnDisable()
        {
            DamageObject.OnPlayerDamage -= DecreaseHealthLevel;
            DamageObjectAOE.OnDamagePlayer -= DecreaseHealthLevel;
            FoodObject.OnFoodPickup -= IncreaseHungerLevel;
            EnemyController.OnPlayerDamage -= DecreaseHealthLevel;
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

            if (healthLevel >= _maxHealth || _takingDamage) return;

            if (_healthModifierReady && hungerLevel > 0 && moistLevel > 0)
            {
                StartCoroutine(HealthRegenCoroutine());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Water"))
            {
                _isMoist = true;
            }
            else if (other.gameObject.CompareTag("Damage"))
            {
                _takingDamage = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Water"))
            {
                _isMoist = false;
            }
            else if (other.gameObject.CompareTag("Damage"))
            {
                _takingDamage = false;
            }
        }
        #endregion
        
        #region Attribute Modifiers
        // Decrease Attributes
        private void DecreaseMoistLevel(int amount)
        {
            if(moistLevel <= 0) return;
            moistLevel -= amount;
            if (moistLevel <= 0)
            {
                moistLevel = 0;
            }
            OnMoistLevelChanged?.Invoke(moistLevel);
        }
        
        private void DecreaseHungerLevel(int amount)
        {
            if (hungerLevel <= 0) return;
            hungerLevel -= amount;
            if (hungerLevel <= 0)
            {
                hungerLevel = 0;
            }
            OnHungerLevelChanged?.Invoke(hungerLevel);
        }

        private void DecreaseHealthLevel(int amount)
        {
            if (healthLevel <= 0) return;
            healthLevel -= amount;
            if (healthLevel <= 0)
            {
                OnGameOver?.Invoke();
            }
            OnHealthLevelChanged?.Invoke(healthLevel);
            OnDamageTaken?.Invoke();
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

        private void IncreaseHungerLevel(int amount)
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
            var waitTime = 0;
            if (inWater)
            {
                IncreaseMoistLevel(moistRegenAmount);
                waitTime = moistRegenInterval;
            }
            else
            {
                DecreaseMoistLevel(moistReductionAmount);
                waitTime = moistReductionAmount;
            }
            
            _moistModifierReady = false;
            yield return new WaitForSeconds(waitTime);
            _moistModifierReady = true;
        }
        private IEnumerator ZeroMoistDamageCoroutine()
        {
            DecreaseHealthLevel(zeroMoistDamage);
            _moistModifierReady = false;
            yield return new WaitForSeconds(moistReductionInterval);
            _moistModifierReady = true;
        }
        
        private IEnumerator HungerCoroutine()
        {
            DecreaseHungerLevel(hungerReductionAmount);
            _hungerModifierReady = false;
            yield return new WaitForSeconds(hungerReductionInterval);
            _hungerModifierReady = true;
        }
        
        private IEnumerator ZeroHungerDamageCoroutine()
        {
            DecreaseHealthLevel(zeroHungerDamage);
            _hungerModifierReady = false;
            yield return new WaitForSeconds(hungerReductionInterval);
            _hungerModifierReady = true;
        }
        
        private IEnumerator HealthRegenCoroutine()
        {
            IncreaseHealthLevel(healthRegenAmount);
            _healthModifierReady = false;
            yield return new WaitForSeconds(healthRegenInterval);
            _healthModifierReady = true;
        }
        #endregion
    }
}
