using System;
using System.Diagnostics;
using CodeBase.DelayRoutines;
using CodeBase.SDK.ADS;
using CodeBase.Services.Pause;
using CodeBase.Services.Sound;
using CodeBase.Tools;

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

        public void ShowRewardAd(Action onSuccessCallback = null, Action onDenyCallback = null)
        {
            OnAddOpened();

            RewardAd(
                () =>
                {
                    OnAdClosed();
                    onSuccessCallback?.Invoke();
                },
                () =>
                {
                    OnAdClosed();
                    onDenyCallback?.Invoke();
                });
        }

        public void ShowInterstitialAd(Action onSuccessCallback = null, Action onDenyCallback = null)
        {
            if (_interstitialAdCooldown.IsActive)
                return;

            _interstitialAdCooldown.Play();
            OnAddOpened();

            InterstitialAd(
                () =>
                {
                    OnAdClosed();
                    onSuccessCallback?.Invoke();
                },
                () =>
                {
                    OnAdClosed();
                    onDenyCallback?.Invoke();
                });
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

            void OnOpenCallback() =>
                isOpened = true;

            void OnRewardedCallback() =>
                isRewarded = true;

            void OnErrorCallback(string error) =>
                onDenyCallback?.Invoke();

            void OnCloseCallback()
            {
                if (isOpened && isRewarded)
                    onCompleteCallback?.Invoke();
                else
                    onDenyCallback?.Invoke();
            }

            _adProvider.ShowRewardAd(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
        }

        private void InterstitialAd(Action onCompleteCallback = null, Action onDenyCallback = null)
        {
            bool isOpened = false;

            void OpenCallback() =>
                isOpened = true;

            void OfflineCallback() =>
                onDenyCallback?.Invoke();

            void ErrorCallback(string error) =>
                onDenyCallback?.Invoke();

            void CloseCallback(bool isShown)
            {
                if (isOpened && isShown)
                    onCompleteCallback?.Invoke();
                else
                    onDenyCallback?.Invoke();
            }

            _adProvider.ShowInterstitialAd(OpenCallback, CloseCallback, ErrorCallback, OfflineCallback);
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