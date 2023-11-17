using System;
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
    [RequireComponent(typeof(PortalEffector))]
    public class LevelTransfer : ObserverTargetExited<TeleportableObserver, ITeleportable>, IActivated
    {
        [SerializeField] private int _toLevelId;

        private GameStateMachine _stateMachine;
        private EnterLevelPanel _enterLevelPanel;
        private IStaticDataService _staticData;
        private IPersistentProgressService _progressService;
        private LevelData _levelData;

        public event Action Activated = () => { };

        public void Construct(GameStateMachine stateMachine, EnterLevelPanel enterLevelPanel,
            IPersistentProgressService progressService)
        {
            enabled = false;
            _progressService = progressService;
            _enterLevelPanel = enterLevelPanel;
            _stateMachine = stateMachine;
            _levelData = GetLevelData();
            TryActivate();
        }

        protected override void OnTriggerObserverEntered(ITeleportable _)
        {
            _enterLevelPanel.Show(_levelData is null ? 0 : _levelData.Stars, _toLevelId);
            _enterLevelPanel.EnterClick += LoadNewLevel;
        }

        protected override void OnTriggerObserverExited(ITeleportable _)
        {
            _enterLevelPanel.Hide();
            _enterLevelPanel.EnterClick -= LoadNewLevel;
        }

        private LevelData GetLevelData() =>
            _progressService.Progress.GlobalData.Levels.Find(data => data.Id == _toLevelId);

        private void LoadNewLevel()
        {
            LoadPayload payload = new LoadPayload(LevelNames.BuildableLevel, true, _toLevelId, true);
            _stateMachine.Enter<LoadLevelState, LoadPayload>(payload);
        }

        private void TryActivate()
        {
            int lastLevel = 0;

            if (_progressService.Progress.GlobalData.Levels.Count > 0)
            {
                lastLevel = _progressService.Progress.GlobalData.Levels.Max(level => level.Id);
            }

            if (_toLevelId <= lastLevel + 1)
            {
                Activate();
            }
        }

        private void Activate()
        {
            Activated.Invoke();
            enabled = true;
        }
    }
}