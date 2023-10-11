using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.Observer;
using CodeBase.Logic.Portal;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LevelTransfer : ObserverTargetExited<TeleportableObserver, ITeleportable>
    {
        [SerializeField] private int _toLevelId;

        private GameStateMachine _stateMachine;
        private EnterLevelPanel _enterLevelPanel;
        private IStaticDataService _staticData;
        private IPersistentProgressService _progressService;

        public void Construct(GameStateMachine stateMachine, EnterLevelPanel enterLevelPanel,
            IPersistentProgressService progressService)
        {
            Debug.Log(progressService);
            _progressService = progressService;
            _enterLevelPanel = enterLevelPanel;
            _stateMachine = stateMachine;
        }

        protected override void OnTriggerObserverEntered(ITeleportable _)
        {
            _enterLevelPanel.Show(GetStarsOnLevel());
            _enterLevelPanel.EnterClick += LoadNewLevel;
        }

        private int GetStarsOnLevel()
        {
            LevelData levelData = _progressService.Progress.GlobalData.Levels.Find(data => data.Id == _toLevelId);
            return levelData is null ? 0 : levelData.Stars;
        }

        protected override void OnTriggerObserverExited(ITeleportable _)
        {
            _enterLevelPanel.Hide();
            _enterLevelPanel.EnterClick -= LoadNewLevel;
        }

        private void LoadNewLevel()
        {
            LoadPayload payload = new LoadPayload(LevelNames.BuildableLevel, true, _toLevelId, true);
            _stateMachine.Enter<LoadLevelState, LoadPayload>(payload);
        }
    }
}