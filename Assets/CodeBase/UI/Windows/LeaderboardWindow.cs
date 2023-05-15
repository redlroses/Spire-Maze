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
        }

        private void SpawnCompetingRanks(SingleRankData[] aroundRanks)
        {
            for (int i = 0; i < aroundRanks.Length; i++)
            {
                GameObject topRankView = _gameUiFactory.CreateRankView(_content);
                RankView rankView = topRankView.GetComponent<RankView>();
                rankView.Set(aroundRanks[i]);

                if (IsMyRank(aroundRanks[i]))
                {
                    rankView.EnableSelfIndication();
                }
            }
        }

        private void SpawnTopRanks(SingleRankData[] topThreeRanks)
        {
            for (int i = 0; i < topThreeRanks.Length; i++)
            {
                GameObject topRankView = _gameUiFactory.CreateTopRankView(i, _content);
                topRankView.GetComponent<TopRankViewConfigurator>().SetUp(i);
                RankView rankView = topRankView.GetComponent<RankView>();
                rankView.Set(topThreeRanks[i]);

                if (IsMyRank(topThreeRanks[i]))
                {
                    rankView.EnableSelfIndication();
                    Debug.Log("my rank " + topThreeRanks[i].Rank);
                }
            }
        }

        private bool IsMyRank(SingleRankData rankData) =>
            rankData.Rank == _currentSelfRank;
    }
}