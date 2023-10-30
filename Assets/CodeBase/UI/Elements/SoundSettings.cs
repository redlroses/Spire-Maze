using CodeBase.Services.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class SoundSettings : MonoBehaviour
    {
        private ISoundService _soundService;

        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Slider _musicSlider;

        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
            Subscribe();
        }

        private void Subscribe()
        {
            _soundSlider.onValueChanged.AddListener(volume =>
                _soundService.SoundVolume(volume / _soundSlider.maxValue));

            _musicSlider.onValueChanged.AddListener(volume =>
                _soundService.MusicVolume(volume / _musicSlider.maxValue));
        }

        private void OnDestroy()
        {
            _soundSlider.onValueChanged.RemoveAllListeners();
            _musicSlider.onValueChanged.RemoveAllListeners();
        }
    }
}