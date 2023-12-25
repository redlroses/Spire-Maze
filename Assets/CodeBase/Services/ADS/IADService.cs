using System;

namespace CodeBase.Services.ADS
{
    public interface IADService : IService
    {
        void ShowRewardAd(Action onSuccessCallback = null, Action onDenyCallback = null);

        void ShowInterstitialAd(Action onSuccessCallback = null, Action onDenyCallback = null);
    }
}