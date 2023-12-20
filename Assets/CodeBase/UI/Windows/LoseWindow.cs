using AYellowpaper;
using CodeBase.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.Hero;
using CodeBase.Services.ADS;
using CodeBase.Services.PersistentProgress;
using CodeBase.Sound;
using CodeBase.Tools;
using CodeBase.UI.Elements;
using CodeBase.UI.Elements.Buttons;
using CodeBase.UI.Elements.Buttons.TransitionButtons;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class LoseWindow : WindowBase
    {
        private const string StaticTextFormat = "{0}/";

        [SerializeField] private MenuButton _menuButton;
        [SerializeField] private RestartButton _restartButton;
        [SerializeField] private ReviveButton _reviveButton;
        [SerializeField] private TextSetterAnimated _scoreText;
        [SerializeField] private TextSetterAnimated _coinsText;
        [SerializeField] private TextSetterAnimated _itemsText;
        [SerializeField] private InterfaceReference<IShowHide, MonoBehaviour> _showHide;
        [SerializeField] private AudioPlayer _audioPlayer;
        [SerializeField] private AudioClip _sound;

        private IPersistentProgressService _progressService;

        private TemporalProgress TemporalProgress => _progressService.TemporalProgress;

        private int LevelId => _progressService.Progress.WorldData.LevelState.LevelId;

        public void Construct(
            IPersistentProgressService progressService,
            IADService adService,
            HeroReviver reviver,
            GameStateMachine stateMachine)
        {
            _progressService = progressService;
            _menuButton.Construct(stateMachine);
            _restartButton.Construct(stateMachine, LevelId);

            _reviveButton.Construct(
                adService,
                reviver,
                () =>
                {
                    Close();
                    stateMachine.Enter<GameLoopState>();
                });
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _audioPlayer.PlayOneShot(_sound);
        }

        protected override void Initialize()
        {
            SetScore();
            SetCoins();
            SetItemsCount();
            _showHide.Value.Show();
        }

        protected override void SubscribeUpdates()
        {
            _menuButton.Subscribe();
            _restartButton.Subscribe();
        }

        protected override void Cleanup()
        {
            _menuButton.Cleanup();
            _restartButton.Cleanup();
            _reviveButton.Cleanup();
        }

        private void SetScore() =>
            _scoreText.SetTextAnimated(TemporalProgress.Score);

        private void SetCoins() =>
            _coinsText.SetTextAnimated(TemporalProgress.CoinsCount);

        private void SetItemsCount()
        {
            _itemsText.SetStaticText(StaticTextFormat + TemporalProgress.TotalArtifactsCount);
            _itemsText.SetTextAnimated(TemporalProgress.CollectedArtifactsCount);
        }
    }
}