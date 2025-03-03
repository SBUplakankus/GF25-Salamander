// ============================================================================================
// CLASS: PlayerCameraShake
// ============================================================================================
// Description:
//   Shakes the player camera at various stages of the game
//
// Methods:
//   - HandleDamageTaken: Triggers the shake event when damage is taken by the player
//
// Related Classes:
//   - PlayerAttributes: Listens in on the damage taken event in PlayerAttributes
// ============================================================================================

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
