using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;

namespace CodeBase.Leaderboard
{
    public class YandexLeaderBoard : ILeaderBoard
    {
        private const string Anonymous = "Anonymous";

        private readonly string _name;
        private readonly int _topPlayersCount;
        private readonly int _competingPlayersCount;
        private readonly bool _isIncludeSelf;

        private List<RanksData> _ranksData;
        private bool _isLeaderboardDataReceived;

        public YandexLeaderBoard(string name, int topPlayersCount, int competingPlayersCount, bool isIncludeSelf = true)
        {
            _name = name;
            _topPlayersCount = topPlayersCount;
            _competingPlayersCount = competingPlayersCount;
            _isIncludeSelf = isIncludeSelf;
        }

        public async Task<RanksData[]> GetLeaderboardEntries()
        {
            throw new NotImplementedException();
            // _isLeaderboardDataReceived = false;
            // TryAuthorize();
            // Leaderboard.GetEntries(_name, OnGetLeaderBoardEntries, null, _topPlayersCount, _competingPlayersCount, _isIncludeSelf);
            //
            // while (_isLeaderboardDataReceived == false)
            // {
            //     await Task.Yield();
            // }
            //
            // return _ranksData.ToArray();
        }

        public void SetScore(int score, string avatarName)
        {
            throw new NotImplementedException();
        //     TryAuthorize();
        //     TryGetPersonalData();
        //
        //     if (PlayerAccount.IsAuthorized == false)
        //         return;
        //
        //     UnityEngine.SocialPlatforms.Impl.Leaderboard.GetPlayerEntry(_name, result =>
        //     {
        //         if (result.score >= score)
        //         {
        //             return;
        //         }
        //
        //         UnityEngine.SocialPlatforms.Impl.Leaderboard.SetScore(_name, score, null, null, avatarName);
        //     });
        }

        // private void TryAuthorize()
        // {
        //     if (PlayerAccount.IsAuthorized)
        //     {
        //         return;
        //     }
        //
        //     PlayerAccount.Authorize();
        // }
        //
        // private void OnGetLeaderBoardEntries(LeaderboardGetEntriesResponse board)
        // {
        //     TryAuthorize();
        //     TryGetPersonalData();
        //
        //     _ranksData = new List<RanksData>(board.entries.Length);
        //
        //     foreach (var entry in board.entries)
        //     {
        //         string name = entry.player.publicName;
        //
        //         if (string.IsNullOrEmpty(name))
        //         {
        //             name = Anonymous;
        //         }
        //
        //         int rank = entry.rank;
        //         int score = entry.score;
        //         string lang = entry.player.lang;
        //         string avatar = entry.extraData;
        //
        //         _ranksData.Add(RanksDataConverter.FromYandex(name, rank, score, lang, avatar));
        //     }
        //
        //     _isLeaderboardDataReceived = true;
        // }
        //
        // private void TryGetPersonalData()
        // {
        //     if (PlayerAccount.IsAuthorized && PlayerAccount.HasPersonalProfileDataPermission == false)
        //     {
        //         PlayerAccount.RequestPersonalProfileDataPermission();
        //     }
        // }
    }
}