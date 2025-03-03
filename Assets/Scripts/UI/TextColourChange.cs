// ============================================================================================
// CLASS: TextColourChange
// ============================================================================================
// Description:
//   Changes the colour of the UI text when the mouse enters
// ============================================================================================

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TextColourChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color32[] textColours;
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _text.color = textColours[0];
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _text.color = textColours[1];
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _text.color = textColours[0];
        }
    }
}
