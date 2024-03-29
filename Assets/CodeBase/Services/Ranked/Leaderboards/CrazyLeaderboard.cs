﻿using CodeBase.Data;
using Cysharp.Threading.Tasks;

namespace CodeBase.Services.Ranked.Leaderboards
{
    public class CrazyLeaderboard : ILeaderboard
    {
        public bool IsAuthorized => false;

        public UniTask<RanksData> GetRanksData() =>
            UniTask.FromResult(new RanksData());

        public UniTask SetScore(int score, string avatarName)
        {
            CrazyGames.CrazyUser.Instance.GetUser(
                user =>
                {
                    if (user != null)
                        CrazyGames.CrazyUser.Instance.AddScore(score);
                });

            return UniTask.CompletedTask;
        }

        public UniTask<bool> TryAuthorize() =>
            UniTask.FromResult(false);

        public UniTask<bool> TryRequestPersonalData() =>
            UniTask.FromResult(false);
    }
}