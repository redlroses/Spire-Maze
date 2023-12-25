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
using JetBrains.Annotations;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(LevelTransferPortalEffector))]
    public class LevelTransfer : ObserverTargetExited<TeleportableObserver, ITeleportable>, IActivated
    {
#if UNITY_EDITOR
        private const bool IsForceActivate = true;
#endif

        [SerializeField] private int _toLevelId;

        private EnterLevelPanel _enterLevelPanel;
        private LevelData _levelData;
        private IPersistentProgressService _progressService;
        private GameStateMachine _stateMachine;
        private IStaticDataService _staticData;

        public event Action Activated = () => { };

        public void Construct(
            GameStateMachine stateMachine,
            EnterLevelPanel enterLevelPanel,
            IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _enterLevelPanel = enterLevelPanel;
            _stateMachine = stateMachine;
            _levelData = GetLevelData();
            enabled = false;

#if UNITY_EDITOR
            if (IsForceActivate)
            {
                Activate();
            }
#else
            TryActivate();
#endif
        }

        protected override void OnTriggerObserverEntered(ITeleportable _)
        {
            _enterLevelPanel.Show(_levelData is null ? 0 : _levelData.Stars, _toLevelId);
            _enterLevelPanel.EnterClicked += LoadNewLevel;
        }

        protected override void OnTriggerObserverExited(ITeleportable _)
        {
            _enterLevelPanel.Hide();
            _enterLevelPanel.EnterClicked -= LoadNewLevel;
        }

        private LevelData GetLevelData() =>
            _progressService.Progress.GlobalData.Levels.Find(data => data.Id == _toLevelId);

        private void LoadNewLevel()
        {
            LoadPayload payload = new LoadPayload(LevelNames.BuildableLevel, _toLevelId, true, true);
            _stateMachine.Enter<LoadLevelState, LoadPayload>(payload);
        }

        [UsedImplicitly]
        private void TryActivate()
        {
            int lastLevel = 0;

            if (_progressService.Progress.GlobalData.Levels.Count > 0)
            {
                lastLevel = _progressService.Progress.GlobalData.Levels.Max(level => level.Id);
                lastLevel = Mathf.Clamp(lastLevel, 0, int.MaxValue);
            }

            if (_toLevelId <= ++lastLevel)
                Activate();
        }

        private void Activate()
        {
            Activated.Invoke();
            enabled = true;
        }
    }
}