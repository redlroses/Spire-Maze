using CodeBase.Logic.MonoStateMachine;
using UnityEngine;

namespace CodeBase.Logic.Enemy.States
{
    public class EnemyWalkState : State
    {
        [SerializeField] private EnemyAnimator _animator;

        private void OnEnable() => _animator.SetWalk();
    }
}