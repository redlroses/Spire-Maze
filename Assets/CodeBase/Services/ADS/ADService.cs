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
        private readonly ISoundService _soundService;
        private readonly IPauseService _pauseService;
        private readonly RoutineSequence _interstitialAdCooldown;
        private readonly Locker _selfLocker = new Locker(nameof(ADService));

        private IADProvider _adProvider;

        public ADService(ISoundService soundService, IPauseService pauseService)
        {
            _soundService = soundService;
            _pauseService = pauseService;
            InitAdProvider();

            _interstitialAdCooldown = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitForSeconds(_adProvider.InterstitialAdCooldownSeconds);
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
#if !UNITY_EDITOR && UNITY_WEBGL && CRAZY_GAMES
            UseCrazyGamesAd();
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

        [Conditional("CRAZY_GAMES")]
        private void UseCrazyGamesAd() =>
            _adProvider = new CrazyGamesAD();

        private void RewardAd(Action onCompleteCallback = null, Action onDenyCallback = null) =>
            _adProvider.ShowRewardAd(onCompleteCallback, _ => onDenyCallback?.Invoke());

        private void InterstitialAd(Action onCompleteCallback = null, Action onDenyCallback = null) =>
            _adProvider.ShowInterstitialAd(onCompleteCallback, _ => onDenyCallback?.Invoke());

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