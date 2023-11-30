using AYellowpaper;
using CodeBase.Tools;
using CodeBase.UI.Elements;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IShowHide> _showHide;
        [SerializeField] private SliderSetter _sliderSetter;
        [SerializeField] private AnimationPlayer _loadingAnimation;

        private void Awake() =>
            DontDestroyOnLoad(this);

        public void Show()
        {
            gameObject.Enable();
            _loadingAnimation.gameObject.Enable();
            _loadingAnimation.Play();
            _sliderSetter.gameObject.Enable();
            _sliderSetter.SetNormalizedValueImmediately(0);
            _showHide.Value.Show();
        }

        public void Hide()
        {
            _loadingAnimation.gameObject.Disable();
            _loadingAnimation.Stop();
            _sliderSetter.gameObject.Disable();
            _showHide.Value.Hide(() => gameObject.Disable());
        }

        public void UpdateLoadProgress(float progress) =>
            _sliderSetter.SetNormalizedValue(progress);
    }
}