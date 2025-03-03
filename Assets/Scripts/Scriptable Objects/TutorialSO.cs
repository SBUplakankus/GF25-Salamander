// ============================================================================================
// CLASS: TutorialSO
// ============================================================================================
// Description:
//   Scriptable Objects containing data on the tutorial pop ups
// ============================================================================================

using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "TutorialSO", menuName = "Scriptable Objects/TutorialSO")]
    public class TutorialSO : ScriptableObject
    {
        [Header("Tutorial Info")]
        public string tutorialHeader;
        [TextArea(2,10)] public string tutorialDetails;
    }
}
