// ============================================================================================
// CLASS: TutorialDisplay
// ============================================================================================
// Description:
//   Handles the UI Elements of the Intro Tutorial. It is a bit scuffed, but it works 
//
// Methods:
//   - DisplayTutorialInfo: Sets the Header and Description based of the Tutorial Scriptable Object
//   - SetTextColour: Sets the colour of the text to green on task completion
//
// Related Classes:
//   - TutorialController: Listens in on the tutorial controllers events so it knows what to display
// ============================================================================================

using System;
using System.Collections;
using PrimeTween;
using Scriptable_Objects;
using Systems;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TutorialDisplay : MonoBehaviour
    {
        [Header("Tutorial Elements")]
        public TMP_Text header;
        public TMP_Text details;
        public TMP_Text skip;
        public Color32[] textColours;

        private const float TutorialFadeCountdown = 2.5f;
        private const float TutorialInterval = 1f;
        
        /// <summary>
        /// When the tutorial pop up has finished its animation and a new one is ready to be shown
        /// </summary>
        public static event Action OnTutorialDisplayEnd;
        public static event Action OnHideTutorialDisplay;

        private void OnEnable()
        {
            SetTextColour(0);
            TutorialController.OnShowNextTutorial += DisplayTutorialInfo;
            TutorialController.OnTutorialTaskCompleted += TutorialTaskCompleted;
            TutorialController.OnTutorialSkip += SkipTutorial;
        }

        private void OnDisable()
        {
            TutorialController.OnShowNextTutorial -= DisplayTutorialInfo;
            TutorialController.OnTutorialTaskCompleted -= TutorialTaskCompleted;
            TutorialController.OnTutorialSkip -= SkipTutorial;
        }

        /// <summary>
        /// Set the text info on the tutorial panel
        /// </summary>
        /// <param name="so">Tutorial SO to display</param>
        private void DisplayTutorialInfo(TutorialSO so)
        {
            SetTextColour(0);
            header.text = so.tutorialHeader;
            details.text = so.tutorialDetails;
        }
        
        /// <summary>
        /// Change text colour and fade away tutorial panel on task completion
        /// </summary>
        private void TutorialTaskCompleted()
        {
            StartCoroutine(TaskCompletionCoroutine());
        }

        private void SetTextColour(int index)
        {
            details.color = textColours[index];
        }

        private void SkipTutorial()
        {
            SkipTutorialText();
            enabled = false;
        }
        
        /// <summary>
        /// Change the colour to green, wait to hide the display then wait for the interval between displays
        /// </summary>
        /// <returns></returns>
        private IEnumerator TaskCompletionCoroutine()
        {
            SetTextColour(1);
            yield return new WaitForSeconds(TutorialFadeCountdown);
            OnHideTutorialDisplay?.Invoke();
            yield return new WaitForSeconds(TutorialInterval);
            OnTutorialDisplayEnd?.Invoke();
        }

        private void SkipTutorialText()
        {
            skip.color = textColours[1];
            skip.text = "Tutorial Skipped";
        }
    }
}
