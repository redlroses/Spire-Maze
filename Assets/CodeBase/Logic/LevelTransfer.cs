using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Portal;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LevelTransfer : ObserverTargetExited<TeleportableObserver, ITeleportable>
    {
        [SerializeField] private int _toLevelId;
        [SerializeField] private TimerOperator _timer;
        [SerializeField] private float _waitDuration;

        private GameStateMachine _stateMachine;

        public void Construct(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _timer.SetUp(_waitDuration, LoadNewLevel);
        }

        protected override void OnTriggerObserverEntered(ITeleportable _)
        {
            _timer.Restart();
            _timer.Play();
        }

        protected override void OnTriggerObserverExited(ITeleportable _)
        {
            _timer.Pause();
        }

        private void LoadNewLevel()
        {
            LoadPayload payload = new LoadPayload(LevelNames.BuildableLevel, true, _toLevelId, true);
            _stateMachine.Enter<LoadLevelState, LoadPayload>(payload);
        }
    }
}