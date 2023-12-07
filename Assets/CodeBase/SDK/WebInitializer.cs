﻿using System;
using System.Collections;
using Agava.YandexGames;
using CodeBase.Infrastructure;
using GameAnalyticsSDK;

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
#endif
            GameAnalytics.Initialize();
            yield return YandexGamesSdk.Initialize();

            while (GameAnalytics.Initialized == false)
                yield return null;

            onReadyCallback?.Invoke();
        }
    }
}