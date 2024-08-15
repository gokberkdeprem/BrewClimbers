using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RopeSwing
{
    public class MiniMapToggleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] GameObject _miniMap; // Assign your Canvas GameObject in the inspector

        private void Start()
        {
            _miniMap.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Button is pressed, enable the canvas
            _miniMap.SetActive(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Button is released, disable the canvas
            _miniMap.SetActive(false);
        }
    }
}
