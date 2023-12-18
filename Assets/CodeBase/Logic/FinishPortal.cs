using System;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Portal;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic
{
    public class FinishPortal : ObserverTargetExited<TeleportableObserver, ITeleportable>
    {
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _delayDuration;

        private GameStateMachine _stateMachine;
        private GameObject _target;

        public event Action Teleported = () => { };

        public void Construct(GameStateMachine stateMachine)
        {
            _timer.SetUp(_delayDuration, EndLevel);
            _stateMachine = stateMachine;
        }

        protected override void OnTriggerObserverEntered(ITeleportable target)
        {
            _target = target.GameObject;
            _timer.Restart();
            _timer.Play();
        }

        protected override void OnTriggerObserverExited(ITeleportable target) =>
            _timer.Pause();

        private void EndLevel()
        {
            _stateMachine.Enter<FinishState, bool>(false);
            Teleported.Invoke();
            _target.Disable();
        }
    }
}