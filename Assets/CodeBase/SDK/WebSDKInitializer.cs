using System;
using System.Collections;
using Agava.YandexGames;
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
            YandexGamesSdk.CallbackLogging = true;
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
            yield return YandexGamesSdk.Initialize();

            while (GameAnalytics.Initialized == false)
                yield return null;

            onReadyCallback?.Invoke();
#endif
        }
    }
}