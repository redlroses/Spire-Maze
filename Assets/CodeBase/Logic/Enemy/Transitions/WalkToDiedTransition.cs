using CodeBase.Logic.Enemy.States;
using CodeBase.Logic.MonoStateMachine;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Transitions
{
    public sealed class WalkToDiedTransition : Transition
    {
        [SerializeField] private EnemyHealth _health;
        
        private void OnEnable()
        {
            _health.Died +=OnDied;
        }

        private void OnDied() => StateMachine.ChangeState<EnemyDiedState>();
    }
}