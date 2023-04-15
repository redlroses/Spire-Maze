using System.Collections.Generic;
using System.Threading.Tasks;
using Agava.YandexGames;
using CodeBase.Data;
using CodeBase.StaticData;
using CodeBase.Tools.Extension;

namespace CodeBase.Leaderboards
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
                var data = new LeaderboardEntryResponse
                {
                    rank = i,
                    extraData = avatars.GetRandom(),
                    score = i * 5,
                    player = new PlayerAccountProfileDataResponse
                    {
                        lang = langs.GetRandom(),
                        publicName = $"Name {i}"
                    },
                };
                
                ranks.Add(data.ToSingleRankData());

                await Task.Delay(150);
            }

            _selfRanksData = ranks.GetRandom();

            _isLeaderboardDataReceived = true;
        }
    }
}