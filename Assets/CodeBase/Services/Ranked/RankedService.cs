using System.Diagnostics;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Leaderboards;
using CodeBase.Services.StaticData;
using Debug = UnityEngine.Debug;

namespace CodeBase.Services.Ranked
{
    public class RankedService : IRankedService
    {
        private const string YandexName = "Yandex";
        private const string EditorName = "Editor";

        private readonly IStaticDataService _staticDataService;

        private ILeaderboard _leaderboard;

        public bool IsAuthorized => _leaderboard.IsAuthorized;

        public RankedService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void InitLeaderboard()
        {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
            UseYandexLeaderboard();
#endif
#if UNITY_EDITOR
            UseEditorLeaderboard();
#endif
        }

        public async Task<bool> Authorize() =>
            await _leaderboard.TryAuthorize();

        public async Task<bool> RequestPersonalData() =>
            await _leaderboard.TryRequestPersonalData();

        public Task<RanksData> GetRanksData() =>
            _leaderboard.GetRanksData();

        public void SetScore(int score, string avatarName = "Test1")
        {
            _leaderboard.SetScore(score, avatarName);
        }


        [Conditional("UNITY_EDITOR")]
        private void UseEditorLeaderboard()
        {
            Debug.Log(nameof(UseEditorLeaderboard));
            _leaderboard = new EditorLeaderboard(_staticDataService.ForLeaderboard(EditorName));
        }

        [Conditional("YANDEX_GAMES")]
        private void UseYandexLeaderboard()
        {
            Debug.Log(nameof(UseYandexLeaderboard));
            _leaderboard = new YandexLeaderboard(_staticDataService.ForLeaderboard(YandexName));
        }
    }
}