using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.AI;
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
        
        
        private NavMeshAgent _navMeshAgent;
        private Rigidbody _rb;
        private EnemyState _enemyState;
        private const int DisabledDuration = 2;
        private const int SlowDownDuration = 3;
        private const int ToxicDamageAmount = 10;
        private const int ToxicDamageInterval = 2;
        private bool _inToxicWaste;
        private bool _damageReady;

        public static event Action<int> OnPlayerDamage; 

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            InitEnemyStats();
        }

        private void Update()
        {
            if (_inToxicWaste && _damageReady)
            {
                StartCoroutine(ToxicDamageCoroutine());
            }
            
            if (_currentHealth <= 0)
            {
                _enemyState = EnemyState.Retreating;
                _currentTarget = retreatPoint;
            }
            
            var playerDistance = GetRemainingDistance(playerPosition);
            var targetDistance = GetRemainingDistance(_currentTarget);
            
            switch (_enemyState)
            {
                case EnemyState.Roaming:
                    if (playerDistance < detectionRange)
                    {
                        Debug.Log("Seen: " + playerDistance);
                        _enemyState = EnemyState.Attacking;
                    }
                    _navMeshAgent.SetDestination(_currentTarget.position);
                    if (targetDistance > 1f) return;
                    UpdatePatrolTarget();
                    break;
                
                case EnemyState.Attacking:
                    if (playerDistance >= detectionRange)
                    {
                        Debug.Log("Escaped: " + playerDistance);
                        _enemyState = EnemyState.Roaming;
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
                    _navMeshAgent.SetDestination(_currentTarget.position);
                    if (targetDistance < 1f)
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
        
        private IEnumerator AttackPlayerCoroutine()
        {
            _navMeshAgent.enabled = false;
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
            _currentHealth -= ToxicDamageAmount;
            yield return new WaitForSeconds(ToxicDamageInterval);
            _damageReady = true;
        }
    }
}
