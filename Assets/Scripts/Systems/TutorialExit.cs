using System;
using UnityEngine;

namespace Systems
{
    public class TutorialExit : MonoBehaviour
    {
        public static event Action OnTutorialZoneExit;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            OnTutorialZoneExit?.Invoke();
        }
    }
}
