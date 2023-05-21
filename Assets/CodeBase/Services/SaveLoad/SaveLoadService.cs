using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.Score;
using CodeBase.Services.Watch;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";

        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;
        private readonly IScoreService _scoreService;
        private readonly IWatchService _watchService;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory, IScoreService scoreService, IWatchService watchService)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
            _scoreService = scoreService;
            _watchService = watchService;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

            _watchService.UpdateProgress();
            _scoreService.UpdateProgress();

            Debug.Log("save");
            PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
        }

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(ProgressKey)?
                .ToDeserialized<PlayerProgress>();
    }
}