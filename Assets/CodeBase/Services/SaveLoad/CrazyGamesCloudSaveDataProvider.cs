using CrazyGames;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
    public class CrazyGamesCloudSaveDataProvider : ICloudSaveDataProvider
    {
        private readonly string _progressKey;

        public CrazyGamesCloudSaveDataProvider(string progressKey)
        {
            _progressKey = progressKey;
        }

        public bool IsPlayerAuthorized()
        {
            bool isAuthorized = false;

            CrazyUser.Instance.GetUser(
                user => isAuthorized = user != null);

            return isAuthorized;
        }

        public UniTask<string> Load() =>
            UniTask.FromResult(PlayerPrefs.GetString(_progressKey, "{}"));

        public void Save(string saveData) =>
            CrazyUser.Instance.SyncUnityGameData();
    }
}