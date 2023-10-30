using System;
using System.Collections.Generic;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Movement;
using CodeBase.Services.Pause;
using JetBrains.Annotations;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class HeroAnimator : MonoCache, IAnimationStateReader, IPauseWatcher
    {
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Running = Animator.StringToHash("Run");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int FallSpeed = Animator.StringToHash("FallSpeed");
        private static readonly int Died = Animator.StringToHash("Died");
        private static readonly int Revive = Animator.StringToHash("Revive");

        private readonly Dictionary<int, AnimatorState> _states = new Dictionary<int, AnimatorState>
        {
            [Animator.StringToHash("Jump")] = AnimatorState.Jump,
            [Animator.StringToHash("Land")] = AnimatorState.Land,
            [Animator.StringToHash("Run")] = AnimatorState.Run,
            [Animator.StringToHash("Dodge")] = AnimatorState.Dodge,
            [Animator.StringToHash("Idle")] = AnimatorState.Idle,
            [Animator.StringToHash("Fall")] = AnimatorState.Fall,
            [Animator.StringToHash("Died")] = AnimatorState.Died,
            [Animator.StringToHash("Revive")] = AnimatorState.Revive,
        };

        [SerializeField] private CustomGravityScaler _gravityScaler;
        [SerializeField] private Animator _animator;

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public event Action StemMoved = () => { };

        public AnimatorState State { get; private set; }

        protected override void FixedRun()
        {
            if ((State == AnimatorState.Fall || State == AnimatorState.Jump) && _gravityScaler.State == GroundState.Grounded)
            {
                PlayLand();
            }

            if (State == AnimatorState.Run && _gravityScaler.State == GroundState.Falling)
            {
                PlayFall();
            }
        }

        public void PlayIdle() =>
            _animator.SetTrigger(Idle);
        public void PlayFall() =>
            _animator.SetTrigger(Fall);

        public void SetSpeed(float speed) =>
            _animator.SetFloat(Speed, speed, 0.01f, Time.deltaTime);

        public void SetFallSpeed(float speed) =>
            _animator.SetFloat(FallSpeed, speed, 0.05f, Time.deltaTime);

        public void PlayJump() =>
            _animator.SetTrigger(Jump);

        public void PlayRun(bool isRun) =>
            _animator.SetBool(Running, isRun);

        public void PlayLand() =>
            _animator.SetTrigger(Land);

        public void PlayDodge() =>
            _animator.SetTrigger(Dodge);

        public void PlayDied() =>
            _animator.SetTrigger(Died);

        public void PlayRevive() =>
            _animator.SetTrigger(Revive);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash) =>
            StateExited?.Invoke(StateFor(stateHash));

        public void Resume() =>
            _animator.enabled = true;

        public void Pause() =>
            _animator.enabled = false;

        [UsedImplicitly]
        private void OnStep() =>
            StemMoved.Invoke();

        private AnimatorState StateFor(int stateHash) =>
            _states.TryGetValue(stateHash, out AnimatorState state) ? state : AnimatorState.Unknown;
    }
}