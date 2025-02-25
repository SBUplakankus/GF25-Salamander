using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        // GPT Code
        [SerializeField] private Transform cameraTransform;

        private void LateUpdate()
        {
            if (cameraTransform)
            {
                transform.LookAt(transform.position + cameraTransform.forward);
            }
        }
    }
}
