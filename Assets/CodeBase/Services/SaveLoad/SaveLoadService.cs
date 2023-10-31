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

            LocalSave(saveData);

#if !UNITY_EDITOR && YANDEX_GAMES
            if (PlayerAccount.IsAuthorized)
            {
                CloudSave(saveData);
            }
#endif
        }

        public async Task<PlayerProgress> LoadProgress()
        {
            string saveData;

#if !UNITY_EDITOR && YANDEX_GAMES
            if (PlayerAccount.IsAuthorized)
            {
                saveData = await CloudLoad();
            }
            else
            {
                saveData = PlayerPrefs.GetString(ProgressKey);
            }
#endif

#if UNITY_EDITOR
            saveData = PlayerPrefs.GetString(ProgressKey);
#endif

            return string.IsNullOrEmpty(saveData)
                ? null
                : saveData.ToDeserialized<PlayerProgress>();
        }

        private void LocalSave(string saveData)
        {
            PlayerPrefs.SetString(ProgressKey, saveData);
            PlayerPrefs.Save();
        }

        private void CloudSave(string saveData)
        {
            Agava.YandexGames.PlayerPrefs.SetString(ProgressKey, saveData);
            Agava.YandexGames.PlayerPrefs.Save(
                () => Debug.Log("Cloud saved successfully"),
                error => Debug.Log($"Cloud save error: {error}"));
        }

        private async Task<string> CloudLoad()
        {
            bool isError = false;
            bool isLoading = true;

            Agava.YandexGames.PlayerPrefs.Load(
                () =>
                {
                    Debug.Log("Cloud loaded successfully");
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

            return isError
                ? PlayerPrefs.GetString(ProgressKey)
                : Agava.YandexGames.PlayerPrefs.GetString(ProgressKey);
        }
    }
}