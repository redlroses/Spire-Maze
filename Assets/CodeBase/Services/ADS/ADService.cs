using System;
using System.Diagnostics;
using CodeBase.DelayRoutines;
using CodeBase.SDK.ADS;
using CodeBase.Services.Pause;
using CodeBase.Services.Sound;
using CodeBase.Tools;
using Debug = UnityEngine.Debug;

namespace CodeBase.Services.ADS
{
    public class ADService : IADService
    {
#if UNITY_EDITOR
        private const int InterstitialAdCooldownSeconds = 5;
#else
        private const int InterstitialAdCooldownSeconds = 60;
#endif

        private readonly ISoundService _soundService;
        private readonly IPauseService _pauseService;
        private readonly RoutineSequence _interstitialAdCooldown;
        private readonly Locker _selfLocker = new Locker(nameof(ADService));

        private IADProvider _adProvider;

        public ADService(ISoundService soundService, IPauseService pauseService)
        {
            _soundService = soundService;
            _pauseService = pauseService;
            _interstitialAdCooldown = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitForSeconds(InterstitialAdCooldownSeconds);
            InitAdProvider();
            _interstitialAdCooldown.Play();
        }

        public void ShowRewardAd(Action onSuccessCallback = null)
        {
            OnAddOpened();

            RewardAd(() =>
                {
                    OnAdClosed();
                    onSuccessCallback?.Invoke();
                },
                OnAdClosed);
        }

        public void ShowInterstitialAd(Action onSuccessCallback = null)
        {
            if (_interstitialAdCooldown.IsActive)
                return;

            _interstitialAdCooldown.Play();
            OnAddOpened();

            InterstitialAd(() =>
                {
                    OnAdClosed();
                    onSuccessCallback?.Invoke();
                },
                OnAdClosed);
        }

        private void InitAdProvider()
        {
#if !UNITY_EDITOR && UNITY_WEBGL && YANDEX_GAMES
            UseYandexAd();
#endif
#if UNITY_EDITOR
            UseEditorAd();
#endif
        }

        [Conditional("UNITY_EDITOR")]
        private void UseEditorAd() =>
            _adProvider = new EditorAD();

        [Conditional("YANDEX_GAMES")]
        private void UseYandexAd() =>
            _adProvider = new YandexAD();

        private void RewardAd(Action onCompleteCallback = null, Action onDenyCallback = null)
        {
            bool isOpened = false;
            bool isRewarded = false;

            Action onOpenCallback = () => isOpened = true;
            Action onRewardedCallback = () => isRewarded = true;
            Action onCloseCallback = () =>
            {
                if (isOpened && isRewarded)
                    onCompleteCallback?.Invoke();
                else
                    onDenyCallback?.Invoke();
            };
            Action<string> onErrorCallback = _ => onDenyCallback.Invoke();
            _adProvider.ShowRewardAd(onOpenCallback, onRewardedCallback, onCloseCallback, onErrorCallback);
        }

        private void InterstitialAd(Action onCompleteCallback = null, Action onDenyCallback = null)
        {
            bool isOpened = false;

            Action openCallback = () => isOpened = true;
            Action<bool> closeCallback = isShown =>
            {
                if (isOpened && isShown)
                    onCompleteCallback?.Invoke();
                else
                    onDenyCallback?.Invoke();
            };
            Action<string> errorCallback = _ => onDenyCallback?.Invoke();
            Action offlineCallback = () => onDenyCallback.Invoke();
            _adProvider.ShowInterstitialAd(openCallback, closeCallback, errorCallback, offlineCallback);
        }

        private void OnAddOpened()
        {
            _soundService.Mute(true, _selfLocker);
            _pauseService.EnablePause(_selfLocker);
        }

        private void OnAdClosed()
        {
            _soundService.Unmute(true, _selfLocker);
            _pauseService.DisablePause(_selfLocker);
        }
    }
}