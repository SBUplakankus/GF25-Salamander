using System;
using UnityEngine;

namespace World
{
    public class FoodObject : MonoBehaviour
    {
        [Header("Food Stats")] 
        [SerializeField] private int foodAmount;

        public static event Action<int> OnFoodPickup;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            OnFoodPickup?.Invoke(foodAmount);
            gameObject.SetActive(false);
        }
    }
}
