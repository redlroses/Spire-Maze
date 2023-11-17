using CodeBase.Infrastructure.States;
using CodeBase.Logic.Movement;
using CodeBase.Logic.Player;
using CodeBase.Logic.StateMachine;
using CodeBase.Logic.StateMachine.States;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Logic.Hero
{
    public class HeroRoot : MonoBehaviour
    {
        [FormerlySerializedAs("_playerHealth")] [SerializeField] private HeroHealth _heroHealth;
        [SerializeField] private HeroReviver _heroReviver;
        [SerializeField] private AliveObserver _aliveObserver;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private HeroMover _mover;
        [SerializeField] private Dodge _dodge;
        [SerializeField] private Jumper _jumper;

        private IPlayerInputService _inputService;
        private PlayerStateMachine _entityStateMachine;

        private void Awake()
        {
            _heroHealth ??= GetComponentInChildren<HeroHealth>();
            _animator ??= GetComponent<HeroAnimator>();
            _heroReviver ??= GetComponent<HeroReviver>();
        }

        private void OnEnable()
        {
            _heroHealth.Died += OnDied;
        }

        private void OnDisable()
        {
            _heroHealth.Died -= OnDied;
        }

        private void OnDestroy()
        {
            _entityStateMachine.Cleanup();
        }

        public void Construct(IPlayerInputService inputService, GameStateMachine stateMachine)
        {
            _inputService = inputService;
            _aliveObserver.Construct(stateMachine);
            _entityStateMachine = new PlayerStateMachine(_animator, _inputService, _mover, _heroHealth, _dodge, _jumper);
            _entityStateMachine.Enter<PlayerIdleState>();
            _heroReviver.Construct(_entityStateMachine);
            _mover.Move(MoveDirection.Stop);
        }

        private void OnDied()
        {
            _entityStateMachine.Enter<DiedState>();
        }
    }
}