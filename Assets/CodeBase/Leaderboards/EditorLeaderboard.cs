using System;
using System.Collections.Generic;
using Agava.YandexGames;
using CodeBase.Data;
using CodeBase.StaticData;
using CodeBase.Tools.Extension;
using Cysharp.Threading.Tasks;

namespace CodeBase.Leaderboards
{
    public class EditorLeaderboard : ILeaderboard
    {
        private readonly int _additiveScoreFactor = 5;
        private readonly int _baseScoreFactor = 10;
        private readonly int _millisecondsDelay = 150;
        private readonly int _ranksDataCount = 6;
        private readonly int _topPlayersCount;
        private readonly List<SingleRankData> _ranksData = new List<SingleRankData>();

        private bool _isLeaderboardDataReceived;
        private SingleRankData _selfRanksData;

        public EditorLeaderboard(LeaderboardStaticData leaderboard)
        {
            _topPlayersCount = leaderboard.TopPlayersCount;
        }

        public bool IsAuthorized { get; private set; }

        public UniTask<bool> TryAuthorize()
        {
            IsAuthorized = true;

            return UniTask.FromResult(true);
        }

        public UniTask<bool> TryRequestPersonalData() =>
            UniTask.FromResult(true);

        public async UniTask<RanksData> GetRanksData()
        {
            _isLeaderboardDataReceived = false;
            ApplyTestData();

            while (_isLeaderboardDataReceived == false)
                await UniTask.Yield();

            return new RanksData(GetTopRanks(), GetCompetingRanks(), _selfRanksData);
        }

        public UniTask SetScore(int score, string avatarName) =>
            UniTask.CompletedTask;

        private SingleRankData[] GetCompetingRanks() =>
            _ranksData.Count > _topPlayersCount
                ? _ranksData.GetRange(_topPlayersCount, _ranksData.Count - _topPlayersCount).ToArray()
                : Array.Empty<SingleRankData>();

        private SingleRankData[] GetTopRanks() =>
            _ranksData.Count == 0
                ? Array.Empty<SingleRankData>()
                : _ranksData.GetRange(0, Math.Min(_topPlayersCount, _ranksData.Count)).ToArray();

        private async void ApplyTestData()
        {
            _ranksData.Clear();

            string[] langs = { "ru", "en", "tr" };
            string[] avatars = { "Test1", "Test2" };

            for (int i = 1; i <= _ranksDataCount; i++)
            {
                LeaderboardEntryResponse data = new LeaderboardEntryResponse
                {
                    rank = i,
                    extraData = avatars.GetRandom(),
                    score = (_baseScoreFactor - i) * _additiveScoreFactor,
                    player = new PlayerAccountProfileDataResponse
                    {
                        lang = langs.GetRandom(),
                        publicName = $"Name {i}",
                    },
                };

                _ranksData.Add(data.ToSingleRankData());
                await UniTask.Delay(_millisecondsDelay);
            }

            _selfRanksData = _ranksData.GetRandom();
            _isLeaderboardDataReceived = true;
        }
    }
}