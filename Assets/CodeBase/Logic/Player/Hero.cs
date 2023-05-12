using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.Movement;
using CodeBase.Logic.StateMachine;
using CodeBase.Logic.StateMachine.States;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private HeroMover _mover;
        [SerializeField] private Dodge _dodge;
        [SerializeField] private Jumper _jumper;

        private IPlayerInputService _inputService;
        private PlayerStateMachine _stateMachine;

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
            _stateMachine = new PlayerStateMachine(_animator, _inputService, _mover, _dodge, _jumper);
            _stateMachine.Enter<PlayerIdleState>();
        }

        private void OnDied()
        {
            _inputService.Cleanup();
            _collider.enabled = false;
            _animator.PlayDied();
        }
    }
}