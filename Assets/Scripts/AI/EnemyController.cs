// ============================================================================================
// CLASS: EnemyController
// ============================================================================================
// Description:
//   Controllers the Snake AI who attack the player throughout the game
//
// Methods:
//   - InitEnemyStats: Initialise all the enemies variables based off of the assigned Scriptable Object
//   - UpdatePatrolTarget: When the enemy reaches it's patrol destination, update its target to a new position
//   - GetRandomPosition: Return a random position from the list of patrol transforms
//   - GetRemainingDistance: Returns a float of the distance from the enemy to the passed through transform.
//                              Used to find distance from player and patrol target
//   - AttackCooldownCoroutine: Resets the enemies attack after a specified time
//   - AttackPlayerCoroutine: Disables the NavMesh temporarily to fling the enemy towards the player before re-enabling
//   - TakeDamage: Called to damage the enemy by a specified amount
//   - ShowHealthBar: Displays the health bar above the Enemy in game
//   - HideHealthBar: Hides the health bar above the Enemy in game
//   - HandleGameOver: Disables the enemy on game over
//
// Related Classes:
//   - GameManager: Disable the enemy on timer expiration
// ============================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Scriptable_Objects;
using Systems;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
    public class EnemyController : MonoBehaviour
    {
        private enum EnemyState { Roaming, Attacking, Retreating }

        [Header("Map Positions")]
        [SerializeField] private Transform playerPosition;
        [SerializeField] private List<Transform> patrolPositions;
        [SerializeField] private Transform retreatPoint;
        private Transform _currentTarget;
        private bool _gameOver;

        [Header("Enemy Stats")]
        [SerializeField] private EnemySO enemyStats;
        [SerializeField] private int detectionRange;
        [SerializeField] private int leapForce;
        [SerializeField] private int leapRange;
        private int _maxHealth;
        private int _currentHealth;
        private int _attackDamage;
        private float _attackInterval;
        private bool _attackReady;

        [Header("Health Bar")] 
        [SerializeField] private Slider hp;
        private const float AnimationDuration = 0.4f;
        private const Ease AnimationEase = Ease.OutCubic;
        private bool _hpViewable;
        
        
        private NavMeshAgent _navMeshAgent;
        private Rigidbody _rb;
        private EnemyState _enemyState;
        private const int DisabledDuration = 2;
        private const int SlowDownDuration = 2;
        private const int ToxicDamageAmount = 10;
        private const int ToxicDamageInterval = 2;
        private bool _inToxicWaste;
        private bool _damageReady;

        public static event Action<int> OnPlayerDamage;
        public static event Action OnEnemyRetreat;
        public static event Action<bool> OnPlayerDetected; 

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            InitEnemyStats();
            GameManager.OnTimerExpiration += HandleGameOver;
        }

        private void OnDisable()
        {
            GameManager.OnTimerExpiration -= HandleGameOver;
        }

        private void Update()
        {
            if (_gameOver) return;
            if (_inToxicWaste && _damageReady)
            {
                StartCoroutine(ToxicDamageCoroutine());
            }
            
            if (_currentHealth <= 0)
            {
                _enemyState = EnemyState.Retreating;
                _currentTarget = retreatPoint;
                HideHealthBar();
                OnEnemyRetreat?.Invoke();
                OnPlayerDetected?.Invoke(true);
            }
            
            var playerDistance = GetRemainingDistance(playerPosition);
            var targetDistance = GetRemainingDistance(_currentTarget);
            
            // The enemy runs off of a state machine that dictates their actions
            // Roaming: Patrols between points in the target list
            // Attacking: Moves towards the player, flinging itself at them when in range
            // Retreating: Runs away to the retreat point where it disappears on arrival
            switch (_enemyState)
            {
                case EnemyState.Roaming:
                    if (playerDistance < detectionRange)
                    {
                        _enemyState = EnemyState.Attacking;
                        OnPlayerDetected?.Invoke(false);
                    }
                    if (_navMeshAgent.enabled)
                    {
                        _navMeshAgent.SetDestination(_currentTarget.position);
                    }
                    if (targetDistance > 5f) return;
                    UpdatePatrolTarget();
                    break;
                
                case EnemyState.Attacking:
                    if (playerDistance >= detectionRange)
                    {
                        _enemyState = EnemyState.Roaming;
                        OnPlayerDetected?.Invoke(true);
                    }
                    if (_navMeshAgent.enabled)
                    {
                        _navMeshAgent.SetDestination(playerPosition.position);
                    }
                    if(!_attackReady || playerDistance > leapRange) return;
                    StartCoroutine(AttackPlayerCoroutine());
                    StartCoroutine(AttackCooldownCoroutine());
                    break;
                
                case EnemyState.Retreating:
                    if (_navMeshAgent.enabled)
                    {
                        _navMeshAgent.speed *= 1.5f;
                        _navMeshAgent.SetDestination(_currentTarget.position);
                    }
                    if (targetDistance < 5f)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player") || !_damageReady) return;
            OnPlayerDamage?.Invoke(_attackDamage);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Food"))
            {
                other.gameObject.SetActive(false);
            }
            else if (other.gameObject.CompareTag("Damage"))
            {
                _inToxicWaste = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Damage")) return;
            _inToxicWaste = false;
        }
        
        /// <summary>
        /// Initialize all the enemies starter values
        /// </summary>
        private void InitEnemyStats()
        {
            _enemyState = EnemyState.Roaming;
            _maxHealth = enemyStats.enemyHealth;
            _currentHealth = _maxHealth;
            _attackDamage = enemyStats.enemyDamage;
            _attackInterval = enemyStats.enemyAttackInterval;
            _attackReady = true;
            _damageReady = true;
            _navMeshAgent.speed = enemyStats.enemySpeed;
            _currentTarget = patrolPositions[0];
            _inToxicWaste = false;
            _gameOver = false;
            hp.maxValue = _maxHealth;
            hp.value = _currentHealth;
            HideHealthBar();
        }

        private void UpdatePatrolTarget()
        {
            var newPoint = GetRandomPosition();
            while (newPoint == _currentTarget)
            {
                newPoint = GetRandomPosition();
            }
            _currentTarget = newPoint;
        }

        private Transform GetRandomPosition()
        {
            var rng = Random.Range(0, patrolPositions.Count);
            return patrolPositions[rng];
        }
        
        
        private float GetRemainingDistance(Transform target)
        {
            return Vector3.Distance(transform.position, target.position);
        }
        
        /// <summary>
        /// Disables the nav mesh and rotates towards the player
        /// Flings itself at the player based on its leap force
        /// Waits for a time before re-enabling
        /// Activates the nav mesh again but is slowed for a time
        /// Returns to normal after this
        /// </summary>
        /// <returns></returns>
        private IEnumerator AttackPlayerCoroutine()
        {
            _navMeshAgent.enabled = false;
            transform.LookAt(playerPosition);
            var direction = (playerPosition.position - transform.position).normalized;
            _rb.isKinematic = false;
            _rb.AddForce(direction * leapForce, ForceMode.Impulse);
            yield return new WaitForSeconds(DisabledDuration);
            
            _rb.isKinematic = true;
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed /= 2;
            yield return new WaitForSeconds(SlowDownDuration);

            _navMeshAgent.speed *= 2;
        }

        private IEnumerator AttackCooldownCoroutine()
        {
            _attackReady = false;
            yield return new WaitForSeconds(_attackInterval);
            _attackReady = true;
        }

        private IEnumerator ToxicDamageCoroutine()
        {
            _damageReady = false;
            TakeDamage(ToxicDamageAmount);
            yield return new WaitForSeconds(ToxicDamageInterval);
            _damageReady = true;
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;
            if (_currentHealth < 0)
            {
                _currentHealth = 0;
            }

            if (!_hpViewable)
            {
                ShowHealthBar();
            }
            
            Tween.UISliderValue(hp, _currentHealth, AnimationDuration, AnimationEase);
        }

        private void ShowHealthBar()
        {
            Tween.Scale(hp.transform, 1, AnimationDuration, AnimationEase);
        }

        private void HideHealthBar()
        {
            _hpViewable = false;
            hp.transform.localScale = Vector3.zero;
        }

        private void HandleGameOver()
        {
            _gameOver = true;
            enabled = false;
        }
    }
}
