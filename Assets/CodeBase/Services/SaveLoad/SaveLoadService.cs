#if UNITY_WEBGL && !UNITY_EDITOR
using Agava.YandexGames;
using Debug = UnityEngine.Debug;
#endif
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Watch;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string EmptySaveString = "{}";
        private const string ProgressKey = "Progress";

        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly IWatchService _watchService;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory,
            IWatchService watchService)
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
            string saveData = _progressService.Progress.ToJson();

            SaveLocal(saveData);

#if !UNITY_EDITOR && YANDEX_GAMES
            if (PlayerAccount.IsAuthorized)
            {
                SaveCloud(saveData);
            }
#endif
        }

        private bool IsSavesEmpty(string saveData) =>
            string.IsNullOrEmpty(saveData) || saveData == EmptySaveString;

        private void SaveLocal(string saveData)
        {
            PlayerPrefs.SetString(ProgressKey, saveData);
            PlayerPrefs.Save();
        }

        private string LoadLocal() =>
            PlayerPrefs.GetString(ProgressKey);

#pragma warning disable CS1998
        public async Task<PlayerProgress> LoadProgress()
        {
#if !UNITY_EDITOR && YANDEX_GAMES
            if (PlayerAccount.IsAuthorized)
            {
                return await GetActualSaveData();
            }
#endif
            string saveData = LoadLocal();

            return IsSavesEmpty(saveData)
                ? null
                : saveData.ToDeserialized<PlayerProgress>();
        }
#pragma warning restore CS1998

#if UNITY_WEBGL && !UNITY_EDITOR
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

        private void SaveCloud(string saveData)
        {
            PlayerAccount.SetCloudSaveData(saveData,
                () => Debug.Log("Cloud saved successfully"),
                error => Debug.Log($"Cloud save error: {error}"));
        }

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
#endif
    }
}