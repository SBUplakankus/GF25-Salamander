// ============================================================================================
// UNUSED
// ============================================================================================

using System;
using UnityEngine;

namespace Systems
{
    public class TutorialZoneBlock : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            Debug.Log("Finish the tutorial");
        }
    }
}
