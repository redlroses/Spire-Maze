using CodeBase.Logic.HealthEntity;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private CapsuleCollider _collider;

        private IPlayerInputService _inputService;

        private void Awake()
        {
            _playerHealth ??= GetComponentInChildren<PlayerHealth>();
            _animator ??= GetComponent<HeroAnimator>();
            _collider ??= GetComponent<CapsuleCollider>();
        }

        private void OnEnable()
        {
            _playerHealth.Died += OnDied;
        }

        private void OnDisable()
        {
            _playerHealth.Died -= OnDied;
        }

        public void Construct(IPlayerInputService inputService)
        {
            _inputService = inputService;
        }

        private void OnDied()
        {
            _inputService.Cleanup();
            _collider.enabled = false;
            _animator.PlayDied();
        }
    }
}