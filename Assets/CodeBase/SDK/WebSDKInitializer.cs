using System;
using System.Collections;
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
using Agava.YandexGames;
#endif
using CodeBase.Infrastructure;
#if !UNITY_EDITOR
using GameAnalyticsSDK;
#endif

namespace CodeBase.SDK
{
    public class WebSDKInitializer
    {
        public WebSDKInitializer()
        {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
            YandexGamesSdk.CallbackLogging = false;
#endif
        }

        public void Start(ICoroutineRunner coroutineRunner, Action onReadyCallback) =>
            coroutineRunner.StartCoroutine(Initialize(onReadyCallback));

        public void Start(ICoroutineRunner coroutineRunner) =>
            coroutineRunner.StartCoroutine(Initialize(null));

        private IEnumerator Initialize(Action onReadyCallback)
        {
#if UNITY_EDITOR
            onReadyCallback?.Invoke();

            yield break;
#else
            GameAnalytics.Initialize();

#if YANDEX_GAMES
            yield return YandexGamesSdk.Initialize();
#endif

            while (GameAnalytics.Initialized == false)
                yield return null;

            onReadyCallback?.Invoke();
#endif
        }
    }
}