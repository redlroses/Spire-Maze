using System;

namespace CodeBase.SDK.ADS
{
    public interface IADProvider
    {
        int InterstitialAdCooldownSeconds { get; }

        void ShowRewardAd(
            Action onCompleteCallback = null,
            Action<string> onDenyCallback = null);

        void ShowInterstitialAd(
            Action onCompleteCallback = null,
            Action<string> onDenyCallback = null);
    }
}