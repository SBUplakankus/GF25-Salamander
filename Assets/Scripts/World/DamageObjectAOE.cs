using System;
using System.Collections;
using UnityEngine;

namespace World
{
    public class DamageObjectAOE : MonoBehaviour
    {
        [SerializeField] private int damageAmount;
        [SerializeField] private int damageInterval;
        private bool _isDamagingPlayer;
        private bool _canDamagePlayer;

        public static event Action<int> OnDamagePlayer; 
        
        private void OnEnable()
        {
            _isDamagingPlayer = false;
            _canDamagePlayer = true;
        }

        private void Update()
        {
            if (!_isDamagingPlayer || !_canDamagePlayer) return;
            StartCoroutine(DamageCoroutine());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _isDamagingPlayer = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _isDamagingPlayer = false;
            }
        }

        private IEnumerator DamageCoroutine()
        {
            _canDamagePlayer = false;
            OnDamagePlayer?.Invoke(damageAmount);
            yield return new WaitForSeconds(damageInterval);
            _canDamagePlayer = true;
        }
    }
}
