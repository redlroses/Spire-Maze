using UnityEngine;

namespace CodeBase.Logic.Сollectible.Chests
{
    [RequireComponent(typeof(ChestAnimator))]
    public class Chest : Collectible
    {
        [SerializeField] private ChestAnimator _animator;

        protected override void OnProgressLoaded(bool isCollected)
        {
            if (isCollected)
                _animator.Open();
        }

        protected override void OnCollected() =>
            Open();

        private void Open() =>
            _animator.Open();
    }
}