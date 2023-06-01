using UnityEngine;

namespace CodeBase.Logic.Enemy.States
{
    public class EnemyFallState : MonoStateMachine.State
    {
        [SerializeField] private EnemyAnimator _animator;

        private void OnEnable() => _animator.SetFall();
    }
}