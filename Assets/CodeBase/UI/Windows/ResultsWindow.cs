using AYellowpaper;
using CodeBase.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.Sound;
using CodeBase.Tools;
using CodeBase.UI.Elements;
using CodeBase.UI.Elements.Buttons.TransitionButtons;
using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ResultsWindow : WindowBase
    {
        [SerializeField] private MenuButton _menuButton;
        [SerializeField] private RestartButton _restartButton;
        [SerializeField] private NextLevelButton _nextLevelButton;
        [SerializeField] private WindowAnimationPlayer _windowAnimationPlayer;
        [SerializeField] private TextSetterAnimated _scoreText;
        [SerializeField] private TextSetterAnimated _itemText;
        [SerializeField] private TextSetterAnimated _coinsText;
        [SerializeField] private StarsView _starsView;
        [SerializeField] private InterfaceReference<IShowHide, MonoBehaviour> _showHide;
        [SerializeField] private AudioPlayer _audioPlayer;
        [SerializeField] private AudioClip _sound;

        private IPersistentProgressService _progressService;

        private int LevelId => _progressService.Progress.WorldData.LevelState.LevelId;
        private TemporalProgress TemporalProgress => _progressService.TemporalProgress;

        protected override void OnAwake()
        {
            base.OnAwake();
            _audioPlayer.PlayOneShot(_sound);
        }

        public void Construct(IPersistentProgressService progressService, GameStateMachine stateMachine,
            IStaticDataService staticData)
        {
            _progressService = progressService;
            _menuButton.Construct(stateMachine);
            _restartButton.Construct(stateMachine, LevelId);
            _nextLevelButton.Construct(stateMachine, staticData, LevelId);
        }

        protected override void Initialize()
        {
            SetScorePoints();
            SetItemsCount();
            SetStars();
            SetCoins();
            _showHide.Value.Show();
        }

        protected override void SubscribeUpdates()
        {
            _menuButton.Subscribe();
            _restartButton.Subscribe();
            _nextLevelButton.Subscribe();
            _windowAnimationPlayer.Play();
        }

        protected override void Cleanup()
        {
            _menuButton.Cleanup();
            _restartButton.Cleanup();
            _nextLevelButton.Cleanup();
        }

        private void SetStars() =>
            _starsView.EnableStars(TemporalProgress.StarsCount);

        private void SetItemsCount() =>
            _itemText.SetTextAnimated(TemporalProgress.ArtifactsCount);

        private void SetScorePoints() =>
            _scoreText.SetTextAnimated(TemporalProgress.Score);

        private void SetCoins() =>
            _coinsText.SetTextAnimated(TemporalProgress.CoinsCount);
    }
}