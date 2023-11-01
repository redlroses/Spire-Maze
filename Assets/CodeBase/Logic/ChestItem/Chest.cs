using CodeBase.Logic.Сollectible;
using UnityEngine;

namespace CodeBase.Logic.ChestItem
{
    [RequireComponent(typeof(ChestAnimator))]
    public class Chest : Collectible
    {
        [SerializeField] private ChestAnimator _animator;

        private Collectible _collectibleItem;

        protected override void OnLoadState(bool isActivated)
        {
            if (isActivated)
            {
                _animator.Open();
            }
        }

        protected override void OnCollected()
        {
            Open();
        }

        private void Open()
        {
            _animator.Open();
        }
    }
}