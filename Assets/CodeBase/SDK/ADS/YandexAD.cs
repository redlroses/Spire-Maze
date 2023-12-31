using System;
using Agava.YandexGames;

namespace CodeBase.SDK.ADS
{
    public class YandexAD : IADProvider
    {
        public void ShowRewardAd(
            Action onCompleteCallback = null,
            Action<string> onDenyCallback = null)
        {
            bool isOpened = false;
            bool isRewarded = false;

            void OnOpenCallback() =>
                isOpened = true;

            void OnRewardedCallback() =>
                isRewarded = true;

            void OnErrorCallback(string error) =>
                onDenyCallback?.Invoke(error);

            void OnCloseCallback()
            {
                if (isOpened && isRewarded)
                    onCompleteCallback?.Invoke();
                else
                    onDenyCallback?.Invoke("Ad was not opened");
            }

            VideoAd.Show(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
        }

        public void ShowInterstitialAd(
            Action onCompleteCallback = null,
            Action<string> onDenyCallback = null)
        {
            bool isOpened = false;

            void OnOpenCallback() =>
                isOpened = true;

            void OnOfflineCallback() =>
                onDenyCallback?.Invoke("App is offline");

            void OnErrorCallback(string error) =>
                onDenyCallback?.Invoke(error);

            void OnCloseCallback(bool isShown)
            {
                if (isOpened && isShown)
                    onCompleteCallback?.Invoke();
                else
                    onDenyCallback?.Invoke("Ad was not opened");
            }

            InterstitialAd.Show(OnOpenCallback, OnCloseCallback, OnErrorCallback, OnOfflineCallback);
        }
    }
}