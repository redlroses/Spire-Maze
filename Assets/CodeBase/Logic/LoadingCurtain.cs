using AYellowpaper;
using CodeBase.Tools;
using CodeBase.UI.Elements;
using Cysharp.Threading.Tasks;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LoadingCurtain : MonoBehaviour
    {
        private const float FinalProgressLoadValue = 1f;
        private const int HideDelay = 1000;
        private const float RotationSpeed = 5;
        private const int RotationDelay = 10;

        [SerializeField] private InterfaceReference<IShowHide> _showHide;
        [SerializeField] private SliderSetter _sliderSetter;
        [SerializeField] private Transform _loadingIcon;

        private void Awake() =>
            DontDestroyOnLoad(this);

        public void Show()
        {
            _loadingIcon.gameObject.Enable();
            gameObject.Enable();
            _sliderSetter.gameObject.Enable();
            _sliderSetter.SetNormalizedValueImmediately(0);
            UpdateLoadProgress(FinalProgressLoadValue);
            Rotate();
            _showHide.Value.ShowInstantly();
        }

        public async void Hide()
        {
            await UniTask.Delay(HideDelay);
            _loadingIcon.gameObject.Disable();
            _sliderSetter.gameObject.Disable();
            _showHide.Value.Hide(() => gameObject.Disable());
        }

        private void UpdateLoadProgress(float progress) =>
            _sliderSetter.SetNormalizedValue(progress);

        private async void Rotate()
        {
            while (_loadingIcon.gameObject.activeInHierarchy)
            {
                _loadingIcon.Rotate(0, 0, RotationSpeed);
                await UniTask.Delay(RotationDelay,
                    DelayType.Realtime,
                    PlayerLoopTiming.Update,
                    destroyCancellationToken);
            }
        }
    }
}