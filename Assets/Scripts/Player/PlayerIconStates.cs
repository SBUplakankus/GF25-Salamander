using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerIconStates : MonoBehaviour
    {
        [Header("Sprite Display")]
        [SerializeField] private Image playerSprite;
    
        [Header("Player Icons")] 
        [SerializeField] private Sprite happy;
        [SerializeField] private Sprite thirsty;
        [SerializeField] private Sprite parched;

        private void Start()
        {
            playerSprite.sprite = happy;
        }

        private void OnEnable()
        {
            PlayerAttributes.OnHealthLevelChanged += HandleHealthChange;
        }

        private void OnDisable()
        {
            PlayerAttributes.OnHealthLevelChanged -= HandleHealthChange;
        }

        private void HandleHealthChange(int amount)
        {
            playerSprite.sprite = amount switch
            {
                > 60 => happy,
                > 30 => thirsty,
                _ => parched
            };
        }
    }
}
