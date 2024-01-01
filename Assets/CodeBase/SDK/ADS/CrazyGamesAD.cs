using System;
using CrazyGames;

namespace CodeBase.SDK.ADS
{
    public class CrazyGamesAD : IADProvider
    {
        public void ShowRewardAd(Action onCompleteCallback = null, Action<string> onDenyCallback = null) =>
            CrazyAds.Instance.beginAdBreakRewarded(
                () => onCompleteCallback?.Invoke(),
                () => onDenyCallback?.Invoke(string.Empty));

        public void ShowInterstitialAd(Action onCompleteCallback = null, Action<string> onDenyCallback = null) =>
            CrazyAds.Instance.beginAdBreak(
                () => onCompleteCallback?.Invoke(),
                () => onDenyCallback?.Invoke(string.Empty));
    }
}