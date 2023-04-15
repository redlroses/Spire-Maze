using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.Ranked;
using UnityEngine;

namespace CodeBase.UI
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private LoadingAnimation _loadingAnimation;

        private IRankedService _rankedService;
        private IGameUiFactory _gameUiFactory;

        private List<RankView> _ranksView;
        private bool _isLeaderboardDataReceived;
        private int _currentSelfRank;

        private void Start()
        {
            Construct(AllServices.Container.Single<IRankedService>(),
                AllServices.Container.Single<IGameUiFactory>());
            
            ShowLeaderBoard();
        }

        public void Construct(IRankedService rankedService, IGameUiFactory gameUiFactory)
        {
            _rankedService = rankedService;
            _gameUiFactory = gameUiFactory;
        }

        public async void ShowLeaderBoard()
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
                GameObject topRankView = _gameUiFactory.CreateRankView();
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
                GameObject topRankView = _gameUiFactory.CreateTopRankView(i);
                topRankView.GetComponent<TopRankViewConfigurator>().SetUp(i);
                RankView rankView = topRankView.GetComponent<RankView>();
                rankView.Set(topThreeRanks[i]);

                if (IsMyRank(topThreeRanks[i]))
                {
                    rankView.EnableSelfIndication();
                }
            }
        }

        private bool IsMyRank(SingleRankData rankData)
        {
            return rankData.Rank == _currentSelfRank;
        }
    }
}