using CodeBase.Logic.Enemy.States;
using CodeBase.Logic.MonoStateMachine;
using CodeBase.Logic.Movement;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Transitions
{
    public sealed class FallToWalkTransition : Transition
    {
        [SerializeField] private EnemyMover _mover;

        private void Update()
        {
            if (_mover.Rigidbody.velocity.y >= 0)
                StateMachine.ChangeState<EnemyWalkState>();
        }
    }
}