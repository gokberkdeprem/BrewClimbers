using TMPro;
using UnityEngine;

namespace UI
{
    public class ComboUI : MonoBehaviour
    {
        private PlayerController _playerController;
        [SerializeField] private GameObject _streakParticle;
        [SerializeField] private TMP_Text _streakText;
        
        
        // Start is called before the first frame update
        void Start()
        {
            _streakParticle.SetActive(false);
            GameManager.Instance.OnPlayerInstantiated.AddListener(OnPlayerInstantiated);
        }

        private void OnPlayerInstantiated()
        {
            _playerController = GameManager.Instance.PlayerController;
            _playerController.OnStreakChanged.AddListener(UpdateStreakText);
        }

        private void UpdateStreakText(int comboCount)
        {
            _streakText.text = "x" + comboCount;
            
            if (comboCount == 0)
            {
                _streakParticle.SetActive(false);
            }
            else
            {
                if(!_streakParticle.activeInHierarchy)
                    _streakParticle.SetActive(true);
            }
        }

    }
}
