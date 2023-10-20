using System;
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

        private List<SingleRankData> _ranksData = new List<SingleRankData>();
        private SingleRankData _selfRanksData;
        private bool _isLeaderboardDataReceived = false;
        private bool _isAuthorized;

        public bool IsAuthorized => _isAuthorized;
        public Task<bool> TryAuthorize()
        {
            _isAuthorized = true;
            return Task.FromResult(true);
        }

        public Task<bool> TryRequestPersonalData()
        {
            return Task.FromResult(true);
        }

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

            ApplyTestData();

            while (_isLeaderboardDataReceived == false)
            {
                await Task.Yield();
            }

            return new RanksData(GetTopRanks(), GetCompetingRanks(), _selfRanksData);
        }

        private SingleRankData[] GetCompetingRanks() =>
            _ranksData.Count > _topPlayersCount
                ? _ranksData.GetRange(_topPlayersCount, _ranksData.Count - _topPlayersCount).ToArray()
                : Array.Empty<SingleRankData>();

        private SingleRankData[] GetTopRanks() =>
            _ranksData.Count == 0
                ? Array.Empty<SingleRankData>()
                : _ranksData.GetRange(0, Math.Min(_topPlayersCount, _ranksData.Count)).ToArray();

        public Task SetScore(int score, string avatarName) =>
            Task.CompletedTask;

        private async void ApplyTestData()
        {
            _ranksData.Clear();

            string[] langs = {"ru", "en", "tr"};
            string[] avatars = {"Test1", "Test2"};

            for (int i = 1; i <= 6; i++)
            {
                var data = new LeaderboardEntryResponse
                {
                    rank = i,
                    extraData = avatars.GetRandom(),
                    score = (10 - i) * 5,
                    player = new PlayerAccountProfileDataResponse
                    {
                        lang = langs.GetRandom(),
                        publicName = $"Name {i}"
                    },
                };

                _ranksData.Add(data.ToSingleRankData());

                await Task.Delay(150);
            }

            _selfRanksData = _ranksData.GetRandom();
            _isLeaderboardDataReceived = true;
        }
    }
}