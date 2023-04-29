using System;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Movement;
using CodeBase.Services.Pause;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class HeroAnimator : MonoCache, IAnimationStateReader, IPauseWatcher
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Running = Animator.StringToHash("Run");
        private static readonly int Dodge = Animator.StringToHash("Dodge");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int FallSpeed = Animator.StringToHash("FallSpeed");
        private static readonly int Died = Animator.StringToHash("Died");

        private readonly int _jumpStateHash = Animator.StringToHash("Jump");
        private readonly int _landStateHash = Animator.StringToHash("Land");
        private readonly int _runStateHash = Animator.StringToHash("Run");
        private readonly int _dodgeStateHash = Animator.StringToHash("Dodge");
        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _fallStateHash = Animator.StringToHash("Fall");
        private readonly int _diedStateHash = Animator.StringToHash("Died");

        [SerializeField] private CustomGravityScaler _gravityScaler;
        [SerializeField] private Animator _animator;
        
        private IPauseReactive _pauseReactive;

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }

        private void OnDestroy()
        {
            _pauseReactive.Pause -= OnPause;
            _pauseReactive.Resume -= OnResume;
        }
        
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
        
        public void RegisterPauseWatcher(IPauseReactive pauseReactive)
        {
            _pauseReactive = pauseReactive;
            _pauseReactive.Pause += OnPause;
            _pauseReactive.Resume += OnResume;
        }

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

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
            {
                state = AnimatorState.Idle;
            }
            else if (stateHash == _runStateHash)
            {
                state = AnimatorState.Run;
            }
            else if (stateHash == _fallStateHash)
            {
                state = AnimatorState.Fall;
            }
            else if (stateHash == _jumpStateHash)
            {
                state = AnimatorState.Jump;
            }
            else if (stateHash == _landStateHash)
            {
                state = AnimatorState.Land;
            }
            else if (stateHash == _dodgeStateHash)
            {
                state = AnimatorState.Dodge;
            }
            else
            {
                state = AnimatorState.Unknown;
            }

            Debug.Log(state);
            return state;
        }
        
        private void OnResume() => _animator.enabled = true;

        private void OnPause() => _animator.enabled = false;
    }
}