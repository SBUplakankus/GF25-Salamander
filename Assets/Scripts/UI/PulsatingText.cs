using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class PulsatingText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private const float GrowSize = 1.04f;
        private const float AnimationDuration = 0.2f;
        private const Ease AnimationEase = Ease.InOutSine;
        private Sequence _sequence;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tween.StopAll();
            transform.localScale = Vector3.one;
        }
    }
}
