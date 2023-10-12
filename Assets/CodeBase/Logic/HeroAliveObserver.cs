using AYellowpaper;
using CodeBase.DelayRoutines;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.HealthEntity;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Logic
{
    public class HeroAliveObserver : MonoBehaviour
    {
        [SerializeField] private float _loseWindowShowDelay = 2f;
        [SerializeField] private InterfaceReference<IHealth, MonoBehaviour> _heroHealth;

        private IWindowService _windowService;

        private RoutineSequence _openLoseWindowRoutine;
        private GameStateMachine _stateMachine;

        private void OnDestroy()
        {
            _openLoseWindowRoutine.Kill();
            _heroHealth.Value.Died -= OpenLoseWindow;
        }

        public void Construct(IWindowService windowService, GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _windowService = windowService;

            _openLoseWindowRoutine = new RoutineSequence()
                .WaitForSeconds(_loseWindowShowDelay)
                .Then(() => _stateMachine.Enter<FinishState, bool>(true));

            _heroHealth.Value.Died += OpenLoseWindow;
        }

        private void OpenLoseWindow()
        {
            _openLoseWindowRoutine.Play();
        }
    }
}