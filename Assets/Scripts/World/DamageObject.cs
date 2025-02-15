using System;
using UnityEngine;

namespace World
{
    public class DamageObject : MonoBehaviour
    {
        [Header("Damage Stats")] 
        [SerializeField] private int damageAmount;

        public static event Action<int> OnPlayerDamage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnPlayerDamage?.Invoke(damageAmount);
                gameObject.SetActive(false);
            }
        }
    }
}
