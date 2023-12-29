using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.DelayRoutines;
using CodeBase.Logic;
using CodeBase.Services.Ranked;
using CodeBase.Services.SaveLoad;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using Cysharp.Threading.Tasks;
using NTC.Global.System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public class LeaderboardWindow : WindowBase
    {
        [SerializeField] private AnimationPlayer _loadingAnimation;
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _loginPanel;
        [SerializeField] private Button _authorizeButton;

        private IRankedService _rankedService;
        private IUIFactory _gameUiFactory;
        private ISaveLoadService _saveLoadService;

        private List<RankView> _ranksView;
        private bool _isLeaderboardDataReceived;
        private int _currentSelfRank;
        private bool _hasMyRankAtTop;

        private RoutineSequence _authorizationAwait;

        public void Construct(IRankedService rankedService, IUIFactory gameUiFactory, ISaveLoadService saveLoadService)
        {
            _rankedService = rankedService;
            _gameUiFactory = gameUiFactory;
            _saveLoadService = saveLoadService;
        }

        protected override void Initialize() =>
            ShowLeaderBoard();

        private async void ShowLeaderBoard()
        {
            if (_rankedService.IsAuthorized == false)
            {
                _loginPanel.Enable();
                _authorizeButton.onClick.AddListener(OnClickAuthorized);

                while (_rankedService.IsAuthorized == false)
                    await UniTask.Yield();

                await _rankedService.RequestPersonalData();
                await _saveLoadService.ActualizeProgress();
            }

            _loginPanel.Disable();
            _loadingAnimation.Play();
            RanksData ranksData = await _rankedService.GetRanksData();
            _loadingAnimation.Stop();
            _currentSelfRank = ranksData.SelfRank.Rank;
            SpawnTopRanks(ranksData.TopThreeRanks);
            SpawnCompetingRanks(ranksData.AroundRanks);

            if (_hasMyRankAtTop == false)
                SpawnRank(CreateCompetingRankView, ranksData.SelfRank);
        }

        private void OnClickAuthorized()
        {
            _authorizeButton.onClick.RemoveAllListeners();
            _rankedService.Authorize();
        }

        private void SpawnCompetingRanks(SingleRankData[] aroundRanks)
        {
            for (int i = 0; i < aroundRanks.Length; i++)
                SpawnRank(CreateCompetingRankView, aroundRanks[i]);
        }

        private void SpawnTopRanks(SingleRankData[] topThreeRanks)
        {
            for (int i = 0; i < topThreeRanks.Length; i++)
            {
                int index = i;
                SpawnRank(() => CreateTopRankView(index), topThreeRanks[index]);
            }
        }

        private GameObject CreateCompetingRankView()
        {
            GameObject rankViewObject = _gameUiFactory.CreateRankView(_content);

            return rankViewObject;
        }

        private GameObject CreateTopRankView(int i)
        {
            GameObject rankViewObject = _gameUiFactory.CreateTopRankView(_content);
            rankViewObject.GetComponent<TopRankViewConfigurator>().SetUp(i);

            return rankViewObject;
        }

        private void SpawnRank(Func<GameObject> createRankView, SingleRankData rankData)
        {
            if (rankData == null || rankData.Score == 0)
                return;

            GameObject rankViewObject = createRankView.Invoke();
            RankView rankView = rankViewObject.GetComponent<RankView>();
            rankView.Set(rankData);

            if (IsMyRank(rankData))
            {
                rankView.EnableSelfIndication();
                _hasMyRankAtTop = true;
            }
        }

        private bool IsMyRank(SingleRankData rankData) =>
            rankData.Rank == _currentSelfRank;
    }
}