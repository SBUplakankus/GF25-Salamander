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
        private bool _tutorialReady;

        [Header("Tutorial Blockers")] 
        [SerializeField] private GameObject tutorialExit;
        [SerializeField] private GameObject tutorialZoneEntry;
        
        public static event Action<TutorialSO> OnShowNextTutorial;
        public static event Action OnTutorialTaskCompleted;
        public static event Action OnTutorialSkip;
        public static event Action OnTutorialEnd;

        private void Start()
        {
            _tutorialIndex = 0;
            _tutorialActive = true;
            _tutorialReady = false;
            ShowNextTutorial();
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
            
            // Checks for user input at each stage of the tutorial then progresses 
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                SkipTutorial();
            }

            if (!_tutorialReady) return;
            
            switch (_tutorialIndex)
            {
                case 0:
                    case 4:
                        case 5:
                            case 6:
                    if (Input.anyKeyDown)
                    {
                        TutorialTaskCompleted();
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S)
                        || Input.GetKeyDown(KeyCode.D))
                    {
                        TutorialTaskCompleted();
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        TutorialTaskCompleted();
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.E))
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
                _tutorialReady = true;
            }
        }
        
        private void TutorialTaskCompleted()
        {
            OnTutorialTaskCompleted?.Invoke();
            _tutorialIndex++;
            _tutorialReady = false;
        }

        private void TutorialCompleted()
        {
            _tutorialActive = false;
            OnTutorialEnd?.Invoke();
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

        private void SkipTutorial()
        {
            TutorialCompleted();
            OnTutorialSkip?.Invoke();
            enabled = false;
        }
        
        
    }
}
