using System;
using System.Collections;
using Agava.YandexGames;
using CodeBase.Infrastructure;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CodeBase.SDK
{
    public class WebInitializer : IDisposable
    {
        public WebInitializer()
        {
            YandexGamesSdk.CallbackLogging = true;
            PlayerAccount.AuthorizedInBackground += OnAuthorizedInBackground;
        }

        public void Start(ICoroutineRunner coroutineRunner, Action onReadyCallback)
        {
            coroutineRunner.StartCoroutine(Initialize(onReadyCallback));
        }

        public void Start(ICoroutineRunner coroutineRunner)
        {
            coroutineRunner.StartCoroutine(Initialize(null));
        }

        private IEnumerator Initialize(Action onReadyCallback)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
#if !UNITY_WEBGL || UNITY_EDITOR
            onReadyCallback?.Invoke();
            yield break;
#endif
            yield return YandexGamesSdk.Initialize();

            onReadyCallback?.Invoke();
            stopWatch.Stop();

            Debug.Log($"Initialize time: {stopWatch.ElapsedMilliseconds}, ticks: {stopWatch.ElapsedTicks}");
        }

        private void OnAuthorizedInBackground()
        {
            Debug.Log($"{nameof(OnAuthorizedInBackground)} {PlayerAccount.IsAuthorized}");
        }

        public void Dispose()
        {
            PlayerAccount.AuthorizedInBackground -= OnAuthorizedInBackground;
        }
    }
}