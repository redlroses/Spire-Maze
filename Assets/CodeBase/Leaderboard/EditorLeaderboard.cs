using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;

namespace CodeBase.Leaderboard
{
    internal class EditorLeaderBoard : ILeaderBoard
    {
        private List<RanksData> _ranksData;
        private bool _isLeaderboardDataReceived = false;

        public async Task<RanksData[]> GetLeaderboardEntries()
        {
            _isLeaderboardDataReceived = false;
            List<RanksData> ranks = new List<RanksData>();
            string[] langs = {"ru", "en", "tr"};
            string[] avatars = {"HotRod", "Buggy", "Muscle"};

            ApplyTestData(langs, avatars, ranks);

            while (_isLeaderboardDataReceived == false)
            {
                await Task.Yield();
            }

            return ranks.ToArray();
        }

        public void SetScore(int score, string avatarName)
        {
        }

        private async void ApplyTestData(string[] langs, string[] avatars, List<RanksData> ranks)
        {
            for (int i = 0; i < 10; i++)
            {
                // var data = RanksDataConverter.FromYandex($"name {i}", i, i * 10, langs.GetRandom(),
                //     avatars.GetRandom());
                // ranks.Add(data);
                
                throw new NotImplementedException();
                
                await Task.Delay(300);
            }

            _isLeaderboardDataReceived = true;
        }
    }
}