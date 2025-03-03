// ============================================================================================
// CLASS: AudioController
// ============================================================================================
// Description:
//   Controls the global audio sources in the game
//
// Methods:
//   - HandleTimerExpiration: Handles the audio played at the end of the game
//   - HandleButtonEnter: Handles the button sfx for when the mouse enters a button
//   - HandleButtonClick: Handles the button sfx for when the mouse clicks a button
//
// Related Classes:
//   - GameManager: Listens in to when the timer expires
//   - ButtonAudio: Listens in on button events
// ============================================================================================

using System;
using Systems;
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
        [SerializeField] private AudioClip buttonSfx;

        private void OnEnable()
        {
            GameManager.OnTimerExpiration += HandleTimerExpiration;
            ButtonAudio.OnButtonEnter += HandleButtonEnter;
            ButtonAudio.OnButtonClick += HandleButtonClick;
        }

        private void OnDisable()
        {
            GameManager.OnTimerExpiration -= HandleTimerExpiration;
            ButtonAudio.OnButtonEnter -= HandleButtonEnter;
            ButtonAudio.OnButtonClick -= HandleButtonClick;
        }
        
        private void HandleTimerExpiration()
        {
            ambientPlayer.volume = 0.3f;
            sfxPlayer.pitch = 1f;
            sfxPlayer.volume = 1.4f;
            sfxPlayer.PlayOneShot(caughtSfx);
        }

        private void HandleButtonEnter()
        {
            sfxPlayer.volume = 0.35f;
            sfxPlayer.pitch = 0.7f;
            sfxPlayer.PlayOneShot(buttonSfx);
        }

        private void HandleButtonClick()
        {
            sfxPlayer.volume = 0.7f;
            sfxPlayer.pitch = 1.1f;
            sfxPlayer.PlayOneShot(buttonSfx);
        }
    }
}
