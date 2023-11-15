using CodeBase.Services.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class SoundSettings : MonoBehaviour
    {
        private ISoundService _soundService;

        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;

        private void OnDestroy()
        {
            _sfxSlider.onValueChanged.RemoveAllListeners();
            _musicSlider.onValueChanged.RemoveAllListeners();
        }

        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;

            _sfxSlider.value = soundService.SfxVolume * _sfxSlider.maxValue;
            _musicSlider.value = soundService.MusicVolume * _sfxSlider.maxValue;

            Subscribe();
        }

        private void Subscribe()
        {
            _sfxSlider.onValueChanged.AddListener(volume =>
                _soundService.SetSfxVolume(volume / _sfxSlider.maxValue));

            _musicSlider.onValueChanged.AddListener(volume =>
                _soundService.SetMusicVolume(volume / _musicSlider.maxValue));
        }
    }
}