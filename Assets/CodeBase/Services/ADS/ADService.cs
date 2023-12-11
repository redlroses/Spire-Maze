﻿using System;
using System.Diagnostics;
using CodeBase.DelayRoutines;
using CodeBase.SDK.ADS;
using CodeBase.Services.Sound;
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

        private readonly RoutineSequence _interstitialAdCooldown;
        private readonly ISoundService _soundService;
        private readonly SoundLocker _soundLocker = new SoundLocker(nameof(ADService));

        private IADProvider _adProvider;

        public ADService(ISoundService soundService)
        {
            _soundService = soundService;
            _interstitialAdCooldown = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitForSeconds(InterstitialAdCooldownSeconds);
            InitAdProvider();
            _interstitialAdCooldown.Play();
        }

        public void ShowRewardAd(Action onSuccessCallback = null)
        {
            _soundService.Mute(true, _soundLocker);

            RewardAd(() =>
                {
                    onSuccessCallback?.Invoke();
                    _soundService.Unmute(true, _soundLocker);
                }, () => _soundService.Unmute(true, _soundLocker)
            );
        }

        public void ShowInterstitialAd(Action onSuccessCallback = null)
        {
            if (_interstitialAdCooldown.IsActive)
                return;

            _interstitialAdCooldown.Play();
            _soundService.Mute(true, _soundLocker);

            InterstitialAd(() =>
                {
                    onSuccessCallback?.Invoke();
                    _soundService.Unmute(true, _soundLocker);
                }, () => _soundService.Unmute(true, _soundLocker)
            );
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

            onDenyCallback += () => Debug.Log("ShowRewardAd denied");

            Action onOpenCallback = () => isOpened = true;
            Action onRewardedCallback = () => isRewarded = true;
            Action onCloseCallback = () =>
            {
                if (isOpened && isRewarded)
                    onCompleteCallback?.Invoke();
                else
                    onDenyCallback?.Invoke();

                Debug.Log($"ShowRewardAd onClose: is Opened: {isOpened}, isRewarded: {isRewarded}");
            };
            Action<string> onErrorCallback = _ => onDenyCallback.Invoke();
            _adProvider.ShowRewardAd(onOpenCallback, onRewardedCallback, onCloseCallback, onErrorCallback);
        }

        private void InterstitialAd(Action onCompleteCallback = null, Action onDenyCallback = null)
        {
            bool isOpened = false;

            onDenyCallback += () => Debug.Log("ShowInterstitialAd denied");

            Action openCallback = () => isOpened = true;
            Action<bool> closeCallback = isShown =>
            {
                if (isOpened && isShown)
                    onCompleteCallback?.Invoke();
                else
                    onDenyCallback?.Invoke();

                Debug.Log($"ShowInterstitialAd onClose : is Opened: {isOpened}, isShown: {isShown}");
            };
            Action<string> errorCallback = _ => onDenyCallback?.Invoke();
            Action offlineCallback = () => onDenyCallback.Invoke();
            _adProvider.ShowInterstitialAd(openCallback, closeCallback, errorCallback, offlineCallback);
        }
    }
}