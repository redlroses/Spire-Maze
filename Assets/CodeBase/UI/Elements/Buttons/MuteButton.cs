using CodeBase.Services.Sound;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements.Buttons
{
    public class MuteButton : ButtonObserver
    {
        private readonly SoundLocker _locker = new SoundLocker(nameof(MuteButton));

        [SerializeField] private Image _buttonIcon;

        [SerializeField] [ShowAssetPreview] private Sprite _muteIcon;
        [SerializeField] [ShowAssetPreview] private Sprite _unmuteIcon;

        private ISoundService _soundService;

        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
            _buttonIcon.sprite = _soundService.IsMuted ? _muteIcon : _unmuteIcon;
            Subscribe();
        }

        private void OnDestroy() =>
            Cleanup();

        protected override void Call() =>
            SwitchMuteState();

        private void SwitchMuteState()
        {
            if (_soundService.IsMuted)
            {
                _soundService.Unmute(true, _locker);
                _buttonIcon.sprite = _unmuteIcon;
            }
            else
            {
                _soundService.Mute(true, _locker);
                _buttonIcon.sprite = _muteIcon;
            }
        }
    }
}