using System.Collections;
using UnityEngine;

namespace Particle
{
    public class TouchParticleEffect : MonoBehaviour
    {
        private PlayerController _playerController;
        [SerializeField] private GameObject _touchParticle;
        [SerializeField] private float _offsetY;
        
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnPlayerInstantiated.AddListener(AssignParticle);
        }
        
        void InstantiateParticle(Transform targetTransform)
        {
            Vector3 targetPos = new Vector3(targetTransform.position.x, targetTransform.position.y + _offsetY,
                targetTransform.position.z);
            Instantiate(_touchParticle, targetPos, _touchParticle.transform.rotation);
        }

        private void AssignParticle()
        {
            _playerController = GameManager.Instance.PlayerController;
            _playerController.OnTouch.AddListener(InstantiateParticle);
        }
    }
}
