using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.StaticData;

namespace CodeBase.Leaderboard
{
    internal class EditorLeaderboard : ILeaderboard
    {
        private readonly int _topPlayersCount;
        private readonly string _name;
        private readonly int _competingPlayersCount;
        private readonly bool _isIncludeSelf;
        
        private List<SingleRankData> _ranksData;
        private SingleRankData _selfRanksData;
        private bool _isLeaderboardDataReceived = false;

        public EditorLeaderboard(LeaderboardStaticData leaderboard)
        {
            _name = leaderboard.Name;
            _topPlayersCount = leaderboard.TopPlayersCount;
            _competingPlayersCount = leaderboard.CompetingPlayersCount;
            _isIncludeSelf = leaderboard.IsIncludeSelf;
        }

        public async Task<RanksData> GetRanksData()
        {
            _isLeaderboardDataReceived = false;
            List<SingleRankData> ranks = new List<SingleRankData>();
            string[] langs = {"ru", "en", "tr"};
            string[] avatars = {"Test1", "Test2"};

            ApplyTestData(langs, avatars, ranks);

            while (_isLeaderboardDataReceived == false)
            {
                await Task.Yield();
            }

            return new RanksData(GetTopRanks(), GetCompetingRanks(), _selfRanksData);
        }

        private SingleRankData[] GetCompetingRanks()
        {
            return _ranksData.GetRange(_topPlayersCount, _ranksData.Count - _topPlayersCount).ToArray();
        }

        private SingleRankData[] GetTopRanks() =>
            _ranksData.GetRange(0, _topPlayersCount).ToArray();
        
        public void SetScore(int score, string avatarName)
        {
        }

        private async void ApplyTestData(string[] langs, string[] avatars, List<SingleRankData> ranks)
        {
            for (int i = 0; i < 10; i++)
            {
                // var data = RanksDataConverter.FromYandex($"name {i}", i, i * 10, langs.GetRandom(),
                //     avatars.GetRandom());
                // ranks.Add(data);

                await Task.Delay(300);
            }

            _isLeaderboardDataReceived = true;
        }
    }
}