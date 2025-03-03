// ============================================================================================
// CLASS: ButtonGrow
// ============================================================================================
// Description:
//   Grows the button size when the mouse enters
// ============================================================================================

using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonGrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const float GrowSize = 1.04f;
        private const float AnimationDuration = 0.15f;
        private const Ease AnimationEase = Ease.Linear;

        private void OnEnable()
        {
            transform.localScale = Vector3.one;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tween.Scale(transform, GrowSize, AnimationDuration, AnimationEase, useUnscaledTime:true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tween.Scale(transform, 1, AnimationDuration, AnimationEase, useUnscaledTime:true);
        }
    }
}
