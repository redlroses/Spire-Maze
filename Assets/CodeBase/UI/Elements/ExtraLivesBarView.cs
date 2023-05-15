using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.UI.Services.Factory;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ExtraLivesBarView : MonoBehaviour
    {
        [SerializeField] private Transform _barContainer;
        [SerializeField] private int _currentLivesCount;
        [SerializeField] private int _currentActiveLivesCount;
        [SerializeField] private List<ExtraLiveView> _extraLives = new List<ExtraLiveView>();

#if UNITY_EDITOR
        [SerializeField] private int _testCurrentActiveLivesCount;
#endif

        private IUIFactory _gameUiFactory;

        private int CurrentMaxLivesCount => _extraLives.Count;

        public void Construct(IUIFactory gameUiFactory)
        {
            _gameUiFactory = gameUiFactory;
        }

        public void SetMaxLivesCount(int count)
        {
            if (count == CurrentMaxLivesCount)
            {
                return;
            }

            if (count > CurrentMaxLivesCount)
            {
                AddMaxLives(count);
            }
            else
            {
                RemoveMaxLives(count);
            }
        }

        public void SetActiveLivesCount(int activeLivesCount)
        {
            if (activeLivesCount == _currentActiveLivesCount)
            {
                return;
            }

            if (activeLivesCount > _currentActiveLivesCount)
            {
                ActivateLives(activeLivesCount);
            }
            else
            {
                DeactivateLives(activeLivesCount);
            }

            _currentActiveLivesCount = activeLivesCount;
        }

        private void DeactivateLives(int activeLivesCount)
        {
            for (int i = _currentActiveLivesCount - 1; i > activeLivesCount - 1; i--)
            {
                _extraLives[i].Deactivate();
            }
        }

        private void ActivateLives(int activeLivesCount)
        {
            for (int i = _currentActiveLivesCount; i <= activeLivesCount - 1; i++)
            {
                _extraLives[i].Activate();
            }
        }

        private void AddMaxLives(int targetLivesCount)
        {
            for (int i = CurrentMaxLivesCount; i < targetLivesCount; i++)
            {
                var extraLifeView = _gameUiFactory.CreateExtraLiveView(_barContainer).GetComponent<ExtraLiveView>();
                _extraLives.Add(extraLifeView);
                extraLifeView.Deactivate();
                SetActiveLivesCount(_currentActiveLivesCount + 1);
            }
        }

        private void RemoveMaxLives(int targetLivesCount)
        {
            for (int i = CurrentMaxLivesCount; i > targetLivesCount; i--)
            {
                var extraLife = _extraLives[i - 1];

                if (extraLife.IsActive)
                {
                    _currentActiveLivesCount--;
                }

                _extraLives.Remove(extraLife);
                Destroy(extraLife.gameObject);
            }
        }

#if UNITY_EDITOR
        [Button("Text Set Lives Count")]
        private void TestSetLivesCount()
        {
            SetMaxLivesCount(_currentLivesCount);
        }

        [Button("Text Set Active Lives Count")]
        private void TestSetActiveLivesCount()
        {
            SetActiveLivesCount(_testCurrentActiveLivesCount);
        }

        [Button("SetUp Max Lives")]
        private void AddMaxLives()
        {
            SetMaxLivesCount(CurrentMaxLivesCount + 1);
        }

        [Button("Remove Max Lives")]
        private void RemoveMaxLives()
        {
            SetMaxLivesCount(CurrentMaxLivesCount - 1);
        }

        [Button("SetUp Active Lives")]
        private void AddActiveLives()
        {
            SetActiveLivesCount(_currentActiveLivesCount + 1);
        }

        [Button("Remove Active Lives")]
        private void RemoveActiveLives()
        {
            SetActiveLivesCount(_currentActiveLivesCount - 1);
        }
#endif
    }
}