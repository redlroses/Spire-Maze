using UnityEngine;

namespace CodeBase.Logic.Portal
{
    [RequireComponent(typeof(Rigidbody))]
    public class Teleportable : MonoBehaviour, ITeleportable
    {
        [SerializeField] private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        public void Teleportation(Vector3 position)
        {
            _rigidbody.position = position;
        }
    }
}
