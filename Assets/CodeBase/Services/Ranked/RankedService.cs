using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Leaderboards;
using CodeBase.Services.StaticData;

namespace CodeBase.Services.Ranked
{
    public class RankedService : IRankedService
    {
        private const string YandexName = "Yandex";
        private const string EditorName = "Editor";

        private readonly IStaticDataService _staticDataService;

        private ILeaderboard _leaderboard;

        public RankedService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            UseYandexLeaderboard();
            UseEditorLeaderboard();
        }

        public Task<RanksData> GetRanksData()
        {
             return _leaderboard.GetRanksData();
        }

        public void SetScore(in int score, string avatarName)
        {
            throw new NotImplementedException();
        }

        [Conditional("UNITY_EDITOR")]
        private void UseEditorLeaderboard()
        {
            _leaderboard = new EditorLeaderboard(_staticDataService.ForLeaderboard(EditorName));
        }

        [Conditional("YANDEX_GAMES")]
        private void UseYandexLeaderboard()
        {
            _leaderboard = new YandexLeaderboard(_staticDataService.ForLeaderboard(YandexName));
        }
    }
}