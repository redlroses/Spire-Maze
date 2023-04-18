using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.StaticData;
using Agava.YandexGames;
using CodeBase.Tools.Extension;

namespace CodeBase.Leaderboards
{
    public class YandexLeaderboard : ILeaderboard
    {
        private readonly string _name;
        private readonly int _topPlayersCount;
        private readonly int _competingPlayersCount;
        private readonly bool _isIncludeSelf;

        private List<SingleRankData> _ranksData;
        private SingleRankData _selfRanksData;
        private bool _isLeaderboardDataReceived;

        public YandexLeaderboard(LeaderboardStaticData leaderboard)
        {
            _name = leaderboard.Name;
            _topPlayersCount = leaderboard.TopPlayersCount;
            _competingPlayersCount = leaderboard.CompetingPlayersCount;
            _isIncludeSelf = leaderboard.IsIncludeSelf;
        }

        public async Task<RanksData> GetRanksData()
        {
            _isLeaderboardDataReceived = false;
            TryAuthorize();

            Leaderboard.GetPlayerEntry(_name, result => { _selfRanksData = result.ToSingleRankData(); });

            Leaderboard.GetEntries(_name, OnGetLeaderBoardEntries, null, _topPlayersCount, _competingPlayersCount,
                _isIncludeSelf);

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
            TryAuthorize();
            TryGetPersonalData();

            if (PlayerAccount.IsAuthorized == false)
                return;

            Leaderboard.GetPlayerEntry(_name, result =>
            {
                if (result.score >= score)
                {
                    return;
                }

                Leaderboard.SetScore(_name, score, null, null, avatarName);
            });
        }

        private void TryAuthorize()
        {
            if (PlayerAccount.IsAuthorized)
            {
                return;
            }

            PlayerAccount.Authorize();
        }

        private void OnGetLeaderBoardEntries(LeaderboardGetEntriesResponse board)
        {
            TryAuthorize();
            TryGetPersonalData();

            _ranksData = new List<SingleRankData>(board.entries.Length);

            foreach (LeaderboardEntryResponse entry in board.entries)
            {
                _ranksData.Add(entry.ToSingleRankData());
            }

            _isLeaderboardDataReceived = true;
        }

        private void TryGetPersonalData()
        {
            if (PlayerAccount.IsAuthorized && PlayerAccount.HasPersonalProfileDataPermission == false)
            {
                PlayerAccount.RequestPersonalProfileDataPermission();
            }
        }
    }
}