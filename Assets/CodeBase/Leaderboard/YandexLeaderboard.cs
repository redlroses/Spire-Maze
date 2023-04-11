using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.StaticData;

namespace CodeBase.Leaderboard
{
    public class YandexLeaderboard : ILeaderboard
    {
        private const string Anonymous = "Anonymous";

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
            // TryAuthorize();
            
            // Leaderboard.GetPlayerEntry(_name, result =>
            // {
            //     _selfRanksData = result;
            // });
            
            // Leaderboard.GetEntries(_name, OnGetLeaderBoardEntries, null, _topPlayersCount, _competingPlayersCount, _isIncludeSelf);
            
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
            throw new NotImplementedException();
        //     TryAuthorize();
        //     TryGetPersonalData();
        //
        //     if (PlayerAccount.IsAuthorized == false)
        //         return;
        //
        //     Leaderboard.GetPlayerEntry(_name, result =>
        //     {
        //         if (result.score >= score)
        //         {
        //             return;
        //         }
        //
        //         SetScore(_name, score, null, null, avatarName);
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