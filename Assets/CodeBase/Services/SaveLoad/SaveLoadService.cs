using System.Linq;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Watch;
using CodeBase.Tools.Extension;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string EmptySaveString = "{}";
        private const string ProgressKey = "Progress";

        private readonly IPersistentProgressService _progressService;
        private readonly IWatchService _watchService;

        private ICloudSaveDataProvider _cloudDataProvider;

        public SaveLoadService(IPersistentProgressService progressService, IWatchService watchService)
        {
            _progressService = progressService;
            _watchService = watchService;
            InitCloudSaveDataProvider();
        }

        private void InitCloudSaveDataProvider()
        {
#if !UNITY_EDITOR && UNITY_WEBGL && YANDEX_GAMES
            _cloudDataProvider = new YandexCloudSaveDataProvider();
#endif
#if !UNITY_EDITOR && UNITY_WEBGL && CRAZY_GAMES
            _cloudDataProvider = new CrazyGamesCloudSaveDataProvider(ProgressKey);
#endif
        }

        public void SaveProgress()
        {
            _progressService.UpdateWriters();
            _watchService.UpdateProgress();
            Save(_progressService.Progress);
        }

#pragma warning disable CS1998
        public async UniTask ActualizeProgress()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            PlayerProgress actualProgress = await GetActualSaveData();
            _progressService.Progress = actualProgress;
            Save(_progressService.Progress);
#endif
        }

#pragma warning disable CS1998
        public async UniTask<PlayerProgress> LoadProgress()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            if (IsPlayerAuthorized())
                return await GetActualSaveData();
#endif
            string saveData = LoadLocal();

            return IsSavesEmpty(saveData)
                ? null
                : saveData.ToDeserialized<PlayerProgress>();
        }

        private void Save(PlayerProgress progress)
        {
            string saveData = progress.ToJson();

            SaveLocal(saveData);

#if !UNITY_EDITOR && UNITY_WEBGL
            if (IsPlayerAuthorized())
                SaveCloud(saveData);
#endif
        }

        private void SaveLocal(string saveData)
        {
            PlayerPrefs.SetString(ProgressKey, saveData);
            PlayerPrefs.Save();
        }

        private string LoadLocal() =>
            PlayerPrefs.GetString(ProgressKey);

#pragma warning restore CS1998

#if !UNITY_EDITOR && UNITY_WEBGL
        private async UniTask<PlayerProgress> GetActualSaveData()
        {
            string cloudSaveData = await LoadCloud();
            string localSaveData = LoadLocal();

            bool isCloudSaveEmpty = IsSavesEmpty(cloudSaveData);
            bool isLocalSaveEmpty = IsSavesEmpty(localSaveData);

            if (isCloudSaveEmpty && isLocalSaveEmpty)
                return null;

            if (isCloudSaveEmpty)
                return localSaveData.ToDeserialized<PlayerProgress>();

            if (isLocalSaveEmpty)
                return cloudSaveData.ToDeserialized<PlayerProgress>();

            PlayerProgress cloudProgress = cloudSaveData.ToDeserialized<PlayerProgress>();
            PlayerProgress localProgress = localSaveData.ToDeserialized<PlayerProgress>();

            return MergeSaves(cloudProgress, localProgress);
        }

        private void SaveCloud(string saveData) =>
            _cloudDataProvider.Save(saveData);

        private async UniTask<string> LoadCloud() =>
            await _cloudDataProvider.Load();
#endif

        [UsedImplicitly]
        private bool IsPlayerAuthorized() =>
            _cloudDataProvider.IsPlayerAuthorized();

        [UsedImplicitly]
        private PlayerProgress MergeSaves(PlayerProgress prioritized, PlayerProgress secondary)
        {
            int largestLevelIndex =
                Mathf.Max(prioritized.GlobalData.Levels.Count, secondary.GlobalData.Levels.Count) - 1;

            int lowestLevelIndexOfBoth =
                Mathf.Min(prioritized.GlobalData.Levels.Count, secondary.GlobalData.Levels.Count) - 1;

            LevelData[] mergedLevelsData = new LevelData[largestLevelIndex];

            for (int index = 0; index <= lowestLevelIndexOfBoth; index++)
            {
                if (prioritized.GlobalData.Levels[index].Score > secondary.GlobalData.Levels[index].Score)
                    mergedLevelsData[index] = prioritized.GlobalData.Levels[index];
                else
                    mergedLevelsData[index] = secondary.GlobalData.Levels[index];
            }

            PlayerProgress actualProgress = prioritized.Relevance > secondary.Relevance ? prioritized : secondary;

            for (int index = lowestLevelIndexOfBoth + 1; index <= largestLevelIndex; index++)
                mergedLevelsData[index] = actualProgress.GlobalData.Levels[index];

            prioritized.GlobalData.Levels = mergedLevelsData.ToList();

            return prioritized;
        }

        private bool IsSavesEmpty(string saveData) =>
            string.IsNullOrEmpty(saveData) || saveData == EmptySaveString;
    }
}