using System;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Portal;
using UnityEngine;

namespace CodeBase.Logic
{
    public class FinishPortal : ObserverTargetExited<TeleportableObserver, ITeleportable>
    {
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _delayDuration;

        private GameStateMachine _stateMachine;

        public event Action Teleported = () => { };

        public void Construct(GameStateMachine stateMachine)
        {
            _timer.SetUp(_delayDuration, EndLevel);
            _stateMachine = stateMachine;
        }

        private void EndLevel()
        {
            _stateMachine.Enter<FinishState, bool>(false);
            Teleported.Invoke();
        }

        protected override void OnTriggerObserverEntered(ITeleportable target)
        {
            _timer.Restart();
        }

        protected override void OnTriggerObserverExited(ITeleportable target)
        {
            _timer.Pause();
        }
    }
}