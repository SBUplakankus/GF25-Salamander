using System;
using UnityEngine;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        [Header("Audio Sources")] 
        [SerializeField] private AudioSource musicPlayer;
        [SerializeField] private AudioSource ambientPlayer;
        [SerializeField] private AudioSource sfxPlayer;

        [Header("Audio Clips")] 
        [SerializeField] private AudioClip caughtSfx;

        private void OnEnable()
        {
            throw new NotImplementedException();
        }

        private void OnDisable()
        {
            throw new NotImplementedException();
        }
        
        private void HandleTimerExpiration()
    }
}
