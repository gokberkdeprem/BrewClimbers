using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RopeSwing
{
    public class ButtonManager : MonoBehaviour
    {
        public UnityEvent OnButtonColorUpdated;
        public UnityEvent OnAllButtonsAreCleared;
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private int _childCount;
        public int GreenColorCount = 0;
        public int ButtonsCount;

        private void Awake()
        {
            ButtonsCount = _buttonsParent.childCount;
        }

        // Start is called before the first frame update
        void Start()
        {
            _childCount = _buttonsParent.childCount;
            
            foreach (Transform child in _buttonsParent)
            {
                ButtonColorController _bcc = child.GetComponent<ButtonColorController>();
                _bcc.OnButtonColorChanged.AddListener(UpdateColorCount);
            }
        }

        private void UpdateColorCount(bool isIncreased)
        {
            if (isIncreased)
                GreenColorCount += 1;
            else
                GreenColorCount -= 1;

            if (GreenColorCount == _buttonsParent.childCount)
            {
                OnAllButtonsAreCleared.Invoke();
                Debug.Log("GameOver");
            }
            
            OnButtonColorUpdated.Invoke();
        }
    }
}
