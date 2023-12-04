using System.Diagnostics;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Leaderboards;
using CodeBase.Services.StaticData;
using Cysharp.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace CodeBase.Services.Ranked
{
    public class RankedService : IRankedService
    {
        private const string YandexName = "Yandex";
        private const string EditorName = "Editor";

        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assetProvider;

        private ILeaderboard _leaderboard;

        public bool IsAuthorized => _leaderboard.IsAuthorized;

        public RankedService(IStaticDataService staticData)
        {
            _staticData = staticData;
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

        public async UniTask<bool> Authorize() =>
            await _leaderboard.TryAuthorize();

        public async UniTask<bool> RequestPersonalData() =>
            await _leaderboard.TryRequestPersonalData();

        public UniTask<RanksData> GetRanksData() =>
            _leaderboard.GetRanksData();

        public void SetScore(int score, string avatarName = "Test1") =>
            _leaderboard.SetScore(score, avatarName);


        [Conditional("UNITY_EDITOR")]
        private void UseEditorLeaderboard()
        {
            Debug.Log(nameof(UseEditorLeaderboard));
            _leaderboard = new EditorLeaderboard(_staticData.GetLeaderboard(EditorName));
        }

        [Conditional("YANDEX_GAMES")]
        private void UseYandexLeaderboard()
        {
            Debug.Log(nameof(UseYandexLeaderboard));
            _leaderboard = new YandexLeaderboard(_staticData.GetLeaderboard(YandexName), _staticData);
        }
    }
}