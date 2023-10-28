using System;
using CodeBase.Data;
using CodeBase.Logic.Player;
using CodeBase.Logic.Сollectible;
using CodeBase.Services.PersistentProgress;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.ChestItem
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(ChestAnimator))]
    public class Chest : MonoCache, IIndexable, ISavedProgress
    {
        private readonly Vector3 _offsetLocalPosition = new Vector3(-0.45f, 0.2f, 0);

        [SerializeField] private ChestAnimator _animator;

        private Collectible _collectibleItem;

        public event Action Opened; 

        public int Id { get; private set; }
        public bool IsActivated { get; private set; }

        public void Initialize(Collectible item)
        {
            //TODO: Можно избавиться от прослойки Collectible и сразу передавать предмет в сундук.
            //TODO: Сам сундук будет реализовать ICollectible, логка будет короче и проще.

            _collectibleItem = item;
            Id = _collectibleItem.Id;
            transform.localPosition += _offsetLocalPosition;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            IndexableState chestState = progress.WorldData.LevelState.Indexables.Find(cell => cell.Id == Id);

            if (chestState == null)
            {
                return;
            }

            if (chestState.IsActivated)
            {
                Open();
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            IndexableState chestState = progress.WorldData.LevelState.Indexables.Find(cell => cell.Id == Id);

            if (chestState == null)
            {
                progress.WorldData.LevelState.Indexables.Add(new IndexableState(Id, IsActivated));
            }
            else
            {
                chestState.IsActivated = IsActivated;
            }
        }

        private void Open()
        {
            IsActivated = true;
            _animator.Open();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Hero hero) == false)
                return;

            hero.InputService.Action += OnAction;

            if (IsActivated)
                return;

            Debug.Log("Нажмите Е чтоб открыть сундук");
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Hero hero) == false)
                return;

            hero.InputService.Action -= OnAction;
        }

        private void OnAction()
        {
            if (IsActivated)
                return;

            _collectibleItem.GetComponent<SphereCollider>().enabled = true;
            Open();
            Opened?.Invoke();
        }
    }
}