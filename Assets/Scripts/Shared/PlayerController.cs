using UnityEngine;
using UnityEngine.Events;

namespace Shared
{
    public abstract class PlayerController : MonoBehaviour
    {
        public UnityEvent<Transform> OnTouch;
    }
}
