using System;
using System.Collections.Generic;
using Scriptable_Objects;
using UI;
using UnityEngine;

namespace Systems
{
    public class TutorialController : MonoBehaviour
    {
        [Header("Tutorial Details")]
        [SerializeField] private List<TutorialSO> tutorialSOs;
        private int _tutorialIndex;
        private bool _tutorialActive;

        public static event Action<TutorialSO> OnShowNextTutorial;
        public static event Action OnTutorialTaskDone; 

        private void Start()
        {
            _tutorialIndex = 0;
            _tutorialActive = true;
        }

        private void OnEnable()
        {
            TutorialDisplay.OnTutorialDisplayEnd += ShowNextTutorial;
        }

        private void OnDisable()
        {
            TutorialDisplay.OnTutorialDisplayEnd -= ShowNextTutorial;
        }

        private void Update()
        {
            if (!_tutorialActive) return;

            switch (_tutorialIndex)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)
                        || Input.GetKeyDown(KeyCode.D))
                    {
                        
                    }

                    break;
            }
        }

        private void ShowNextTutorial()
        {
            OnShowNextTutorial?.Invoke(tutorialSOs[_tutorialIndex]);
        }
        
        
    }
}
