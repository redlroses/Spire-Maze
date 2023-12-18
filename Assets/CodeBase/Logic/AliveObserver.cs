using AYellowpaper;
using CodeBase.DelayRoutines;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.HealthEntity;
using UnityEngine;

namespace CodeBase.Logic
{
    public class AliveObserver : MonoBehaviour
    {
        [SerializeField] private float _loseWindowShowDelay = 2f;
        [SerializeField] private InterfaceReference<IHealth, MonoBehaviour> _health;

        private RoutineSequence _openLoseWindowRoutine;
        private GameStateMachine _stateMachine;

        private void OnDestroy()
        {
            _openLoseWindowRoutine.Kill();
            _health.Value.Died -= OpenLoseWindow;
        }

        public void Construct(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _openLoseWindowRoutine = new RoutineSequence()
                .WaitForSeconds(_loseWindowShowDelay)
                .Then(() => _stateMachine.Enter<FinishState, bool>(true));

            _health.Value.Died += OpenLoseWindow;
        }

        private void OpenLoseWindow() =>
            _openLoseWindowRoutine.Play();
    }
}