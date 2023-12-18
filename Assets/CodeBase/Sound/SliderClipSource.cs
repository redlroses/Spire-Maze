using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Sound
{
    public class SliderClipSource : AudioClipSource
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private UiSoundConfig _clipConfig;

        private void OnEnable() =>
            _slider.onValueChanged.AddListener(OnMoved);

        private void OnDisable() =>
            _slider.onValueChanged.RemoveListener(OnMoved);

        private void OnMoved(float value) =>
            PlayOneShot(_clipConfig.SliderClip);
    }
}