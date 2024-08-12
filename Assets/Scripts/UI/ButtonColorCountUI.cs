using RopeSwing;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class ButtonColorCountUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _buttonCounterText;
        [SerializeField] private ButtonManager _buttonManager;
        
        // Start is called before the first frame update
        void Start()
        {
            UpdateButtonCounterText();
            _buttonManager.OnButtonColorUpdated.AddListener(UpdateButtonCounterText);
        }

        private void UpdateButtonCounterText()
        {
            _buttonCounterText.text = $" {_buttonManager.GreenColorCount.ToString()}/{_buttonManager.ButtonsCount.ToString()}";
        }
    }
}
