using System;
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
        private bool _isAuthorized;

        public YandexLeaderboard(LeaderboardStaticData leaderboard)
        {
            _name = leaderboard.Name;
            _topPlayersCount = leaderboard.TopPlayersCount;
            _competingPlayersCount = leaderboard.CompetingPlayersCount;
            _isIncludeSelf = leaderboard.IsIncludeSelf;
            CheckAuthorization();
            PlayerAccount.AuthorizedInBackground += CheckAuthorization;
        }

        public bool IsAuthorized => _isAuthorized;

        public async Task<RanksData> GetRanksData()
        {
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

            if (IsAuthorized == false)
            {
                _unsavedScore = score;
                _unsavedAvatarName = avatarName;
                return;
            }

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

            if (_isAuthorized)
            {
                return true;
            }

            PlayerAccount.Authorize(() => isSuccess = true, _ => isError = true);

            while (isSuccess == false && isError == false)
            {
                await Task.Yield();
            }

            _isAuthorized = PlayerAccount.IsAuthorized;
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

        public void CheckAuthorization()
        {
            Debug.Log($"CheckAuthorization invoke: {PlayerAccount.IsAuthorized}");
            _isAuthorized = PlayerAccount.IsAuthorized;
        }

        private SingleRankData[] GetCompetingRanks() =>
            _ranksData.Count > _topPlayersCount
                ? _ranksData.GetRange(_topPlayersCount, _ranksData.Count - _topPlayersCount).ToArray()
                : Array.Empty<SingleRankData>();

        private SingleRankData[] GetTopRanks() =>
            _ranksData.Count == 0
                ? Array.Empty<SingleRankData>()
                : _ranksData.GetRange(0, Math.Min(_topPlayersCount, _ranksData.Count)).ToArray();

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