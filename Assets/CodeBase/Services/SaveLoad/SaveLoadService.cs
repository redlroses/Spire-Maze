using System.Threading.Tasks;
using Agava.YandexGames;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Watch;
using CodeBase.Tools.Extension;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string EmptySaveString = "{}";
        private const string ProgressKey = "Progress";

        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly IWatchService _watchService;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory, IWatchService watchService)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
            _watchService = watchService;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

            _watchService.UpdateProgress();

            Debug.Log("save");

            string saveData = _progressService.Progress.ToJson();

            SaveLocal(saveData);

#if !UNITY_EDITOR && YANDEX_GAMES
            if (PlayerAccount.IsAuthorized)
            {
                SaveCloud(saveData);
            }
#endif
        }

        public async Task<PlayerProgress> LoadProgress()
        {
            string saveData;

#if !UNITY_EDITOR && YANDEX_GAMES
            if (PlayerAccount.IsAuthorized)
            {
                return await GetActualSaveData();
            }
#endif
            saveData = LoadLocal();

            return IsSavesEmpty(saveData)
                ? null
                : saveData.ToDeserialized<PlayerProgress>();
        }

        private async Task<PlayerProgress> GetActualSaveData()
        {
            string cloudSaveData = await LoadCloud();
            string localSaveData = LoadLocal();

            bool isCloudSaveEmpty = IsSavesEmpty(cloudSaveData);
            bool isLocalSaveEmpty = IsSavesEmpty(localSaveData);

            if (isCloudSaveEmpty && isLocalSaveEmpty)
            {
                return null;
            }

            if (isCloudSaveEmpty)
            {
                return localSaveData.ToDeserialized<PlayerProgress>();
            }

            if (isLocalSaveEmpty)
            {
                return cloudSaveData.ToDeserialized<PlayerProgress>();
            }

            PlayerProgress cloudProgress = cloudSaveData.ToDeserialized<PlayerProgress>();
            PlayerProgress localProgress = localSaveData.ToDeserialized<PlayerProgress>();

            return cloudProgress.Relevance > localProgress.Relevance
                ? cloudProgress
                : localProgress;
        }

        private bool IsSavesEmpty(string saveData) =>
            string.IsNullOrEmpty(saveData) || saveData == EmptySaveString;

        private void SaveLocal(string saveData)
        {
            PlayerPrefs.SetString(ProgressKey, saveData);
            PlayerPrefs.Save();
        }

        private void SaveCloud(string saveData)
        {
            PlayerAccount.SetCloudSaveData(saveData,
                () => Debug.Log("Cloud saved successfully"),
                error => Debug.Log($"Cloud save error: {error}"));
        }

        private string LoadLocal() =>
            PlayerPrefs.GetString(ProgressKey);

        private async Task<string> LoadCloud()
        {
            bool isError = false;
            bool isLoading = true;
            string saveData = null;

            PlayerAccount.GetCloudSaveData(
                saves =>
                {
                    Debug.Log("Cloud loaded successfully");
                    saveData = saves;
                    isLoading = false;
                },
                error =>
                {
                    Debug.Log($"Cloud load error: {error}");
                    isError = true;
                    isLoading = false;
                });

            while (isLoading)
            {
                await Task.Yield();
            }

            return isError ? null : saveData;
        }
    }
}