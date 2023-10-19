using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.StaticData;
using Agava.YandexGames;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Leaderboards
{
    public class YandexLeaderboard : ILeaderboard
    {
        private readonly string _name;
        private readonly int _topPlayersCount;
        private readonly int _competingPlayersCount;
        private readonly bool _isIncludeSelf;

        private int _unsavedScore;
        private string _unsavedAvatarName;

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
            bool isAuthorized = await TryAuthorize();

            if (isAuthorized)
            {
                await TryRequestPersonalData();
            }

            if (_unsavedScore != 0)
            {
                await SetScore(_unsavedScore, _unsavedAvatarName);
                _unsavedScore = 0;
            }

            _isLeaderboardDataReceived = false;
            Leaderboard.GetPlayerEntry(_name, result => _selfRanksData = result.ToSingleRankData());
            Leaderboard.GetEntries(_name, OnGetLeaderBoardEntries, null, _topPlayersCount, _competingPlayersCount,
                _isIncludeSelf);

            while (_isLeaderboardDataReceived == false)
            {
                await Task.Yield();
            }

            return new RanksData(GetTopRanks(), GetCompetingRanks(), _selfRanksData);
        }

        public async Task SetScore(int score, string avatarName)
        {
            bool isComplete = false;

            if (PlayerAccount.IsAuthorized == false)
            {
                _unsavedScore = score;
                _unsavedAvatarName = avatarName;
                return;
            }

            // await TryRequestPersonalData();

            Leaderboard.GetPlayerEntry(_name, result =>
            {
                if (result.score >= score)
                {
                    return;
                }

                Leaderboard.SetScore(_name, score, () => isComplete = true, _ => isComplete = true, avatarName);
            }, _ => isComplete = true);

            while (isComplete == false)
            {
                await Task.Yield();
            }
        }

        public async Task<bool> TryAuthorize()
        {
            bool isSuccess = false;
            bool isError = false;

            if (PlayerAccount.IsAuthorized)
                return true;

            PlayerAccount.Authorize(() => isSuccess = true, _ => isError = true);

            while (isSuccess == false && isError == false)
            {
                await Task.Yield();
            }

            Debug.Log($"{nameof(TryAuthorize)}: isSuccess: {isSuccess}, isError: {isError}");
            return isSuccess;
        }

        public async Task<bool> TryRequestPersonalData()
        {
            bool isSuccess = false;
            bool isError = false;

            if (PlayerAccount.IsAuthorized == false)
                return false;

            if (PlayerAccount.HasPersonalProfileDataPermission)
                return true;

            PlayerAccount.RequestPersonalProfileDataPermission(() => isSuccess = true, _ => isError = true);

            while (isSuccess == false && isError == false)
            {
                await Task.Yield();
            }

            Debug.Log($"{nameof(TryRequestPersonalData)}: isSuccess: {isSuccess}, isError: {isError}");
            return isSuccess;
        }

        private SingleRankData[] GetCompetingRanks() =>
            _ranksData.GetRange(_topPlayersCount, _ranksData.Count - _topPlayersCount).ToArray();

        private SingleRankData[] GetTopRanks() =>
            _ranksData.GetRange(0, _topPlayersCount).ToArray();

        private void OnGetLeaderBoardEntries(LeaderboardGetEntriesResponse board)
        {
            _ranksData = new List<SingleRankData>(board.entries.Length);

            foreach (LeaderboardEntryResponse entry in board.entries)
            {
                _ranksData.Add(entry.ToSingleRankData());
            }

            _isLeaderboardDataReceived = true;
        }
    }
}