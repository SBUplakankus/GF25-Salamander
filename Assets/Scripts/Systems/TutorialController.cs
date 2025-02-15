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
        [SerializeField] private List<TutorialSO> tutorials;
        private int _tutorialIndex;
        private bool _tutorialActive;

        [Header("Tutorial Blockers")] 
        [SerializeField] private GameObject tutorialExit;
        [SerializeField] private GameObject tutorialZoneEntry;
        
        public static event Action<TutorialSO> OnShowNextTutorial;
        public static event Action OnTutorialTaskCompleted;

        private void Start()
        {
            _tutorialIndex = 0;
            _tutorialActive = true;
            ShowNextTutorial();
            CloseTutorialZone();
        }

        private void OnEnable()
        {
            TutorialDisplay.OnTutorialDisplayEnd += ShowNextTutorial;
            TutorialExit.OnTutorialZoneExit += CloseTutorialZone;
        }

        private void OnDisable()
        {
            TutorialDisplay.OnTutorialDisplayEnd -= ShowNextTutorial;
            TutorialExit.OnTutorialZoneExit -= CloseTutorialZone;
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
                        TutorialTaskCompleted();
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        TutorialTaskCompleted();
                    }
                    break;
            }
        }

        private void ShowNextTutorial()
        {
            if (_tutorialIndex >= tutorials.Count)
            {
                TutorialCompleted();
            }
            else
            {
                OnShowNextTutorial?.Invoke(tutorials[_tutorialIndex]);
            }
        }

        private void TutorialTaskCompleted()
        {
            OnTutorialTaskCompleted?.Invoke();
            _tutorialIndex++;
        }

        private void TutorialCompleted()
        {
            OpenTutorialZone();
        }
        
        private void CloseTutorialZone()
        {
            tutorialExit.SetActive(true);
            tutorialZoneEntry.SetActive(true);
        }

        private void OpenTutorialZone()
        {
            tutorialExit.SetActive(false);
            tutorialZoneEntry.SetActive(false);
        }
        
        
    }
}
