﻿using UnityEngine;

namespace CodeBase.Logic.Сollectible.Chests
{
    [RequireComponent(typeof(ChestAnimator))]
    public class Chest : Collectible
    {
        [SerializeField] private ChestAnimator _animator;

        private Collectible _collectibleItem;

        protected override void OnProgressLoaded(bool isActivated)
        {
            if (isActivated)
                _animator.Open();
        }

        protected override void OnCollected() =>
            Open();

        private void Open() =>
            _animator.Open();
    }
}