// ============================================================================================
// CLASS: ButtonAudio
// ============================================================================================
// Description:
//   Invokes events based on the mouses interaction with a button
// ============================================================================================

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Audio
{
    public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public static event Action OnButtonEnter;
        public static event Action OnButtonClick;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnButtonEnter?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnButtonClick?.Invoke();
        }
    }
}
