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
            StartCoroutine(LateStart());
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void InstantiateParticle(Transform targetTransform)
        {
            Vector3 targetPos = new Vector3(targetTransform.position.x, targetTransform.position.y + _offsetY,
                targetTransform.position.z);
            Instantiate(_touchParticle, targetPos, _touchParticle.transform.rotation);
        }

        IEnumerator LateStart()
        {
            yield return new WaitForSeconds(1);
            _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _playerController.OnTouch.AddListener(InstantiateParticle);
        }
        
    }
}
