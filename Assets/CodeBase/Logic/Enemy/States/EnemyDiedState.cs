using CodeBase.Logic.MonoStateMachine;
using UnityEngine;

namespace CodeBase.Logic.Enemy.States
{
    public class EnemyDiedState : State
    {
        [SerializeField] private EnemyAnimator _animator;

        private void OnEnable() => _animator.SetDied();
    }
}