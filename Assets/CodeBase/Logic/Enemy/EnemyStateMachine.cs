using System;
using System.Collections.Generic;
using CodeBase.Logic.Enemy.States;
using CodeBase.Logic.MonoStateMachine;

namespace CodeBase.Logic.Enemy
{
    public class EnemyStateMachine : MonoStateMachine.MonoStateMachine
    {
        protected override void InitTransitions()
        {
            foreach (ITransition transition in GetComponentsInChildren<ITransition>())
            {
                transition.Init(this);
            }
        }

        protected override void SetDefaultState() =>
            ChangeState<EnemyWalkState>();

        protected override Dictionary<Type, IMonoState> GetStates() =>
            new Dictionary<Type, IMonoState>
            {
                [typeof(EnemyWalkState)] = GetComponentInChildren<EnemyWalkState>(),
                [typeof(EnemyAttackState)] = GetComponentInChildren<EnemyAttackState>(),
                [typeof(EnemyFallState)] = GetComponentInChildren<EnemyFallState>(),
                [typeof(EnemyDiedState)] = GetComponentInChildren<EnemyDiedState>(),
            };
    }
}