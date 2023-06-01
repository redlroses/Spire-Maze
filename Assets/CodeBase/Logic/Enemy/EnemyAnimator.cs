using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        private const float NormalizedTransitionDuration = 0.1f;
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Died = Animator.StringToHash("Died");

        [SerializeField] private Animator _animator;

        public void SetWalk() => _animator.CrossFade(Walk, NormalizedTransitionDuration);
        public void SetAttack() => _animator.CrossFade(Attack, NormalizedTransitionDuration);
        public void SetFall() => _animator.CrossFade(Fall, NormalizedTransitionDuration);
        public void SetDied() => _animator.CrossFade(Died, NormalizedTransitionDuration);
        
    }
}