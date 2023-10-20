using System;
using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Services.Ranked;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class LeaderboardWindow : WindowBase
    {
        [SerializeField] private LoadingAnimation _loadingAnimation;
        [SerializeField] private Transform _content;

        private IRankedService _rankedService;
        private IUIFactory _gameUiFactory;

        private List<RankView> _ranksView;
        private bool _isLeaderboardDataReceived;
        private int _currentSelfRank;
        private bool _hasMyRankAtTop;

        public void Construct(IRankedService rankedService, IUIFactory gameUiFactory)
        {
            _rankedService = rankedService;
            _gameUiFactory = gameUiFactory;
        }

        protected override void Initialize() =>
            ShowLeaderBoard();

        private async void ShowLeaderBoard()
        {
            _loadingAnimation.Play();
            RanksData ranksData = await _rankedService.GetRanksData();
            _loadingAnimation.Stop();
            _currentSelfRank = ranksData.SelfRank.Rank;
            SpawnTopRanks(ranksData.TopThreeRanks);
            SpawnCompetingRanks(ranksData.AroundRanks);

            if (_hasMyRankAtTop == false)
            {
                SpawnRank(CreateCompetingRankView, ranksData.SelfRank);
            }
        }

        private void SpawnCompetingRanks(SingleRankData[] aroundRanks)
        {
            for (int i = 0; i < aroundRanks.Length; i++)
            {
                SpawnRank(CreateCompetingRankView, aroundRanks[i]);
            }
        }

        private void SpawnTopRanks(SingleRankData[] topThreeRanks)
        {
            for (int i = 0; i < topThreeRanks.Length; i++)
            {
                SpawnRank(() => CreateTopRankView(i), topThreeRanks[i]);
            }
        }

        private GameObject CreateCompetingRankView()
        {
            GameObject rankViewObject = _gameUiFactory.CreateRankView(_content);
            return rankViewObject;
        }

        private GameObject CreateTopRankView(int i)
        {
            GameObject rankViewObject = _gameUiFactory.CreateTopRankView(i, _content);
            rankViewObject.GetComponent<TopRankViewConfigurator>().SetUp(i);
            return rankViewObject;
        }

        private void SpawnRank(Func<GameObject> createRankView, SingleRankData rankData)
        {
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