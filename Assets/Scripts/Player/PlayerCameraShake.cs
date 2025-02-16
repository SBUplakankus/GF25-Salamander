using PrimeTween;
using UnityEngine;

namespace Player
{
    public class PlayerCameraShake : MonoBehaviour
    {
        [SerializeField] private ShakeSettings shakeSettings;
        private void OnEnable()
        {
            PlayerAttributes.OnDamageTaken += HandleDamageTaken;
        }

        private void OnDisable()
        {
            PlayerAttributes.OnDamageTaken -= HandleDamageTaken;
        }

        private void HandleDamageTaken()
        {
            Tween.ShakeLocalPosition(transform, shakeSettings);
        }
    }
}
