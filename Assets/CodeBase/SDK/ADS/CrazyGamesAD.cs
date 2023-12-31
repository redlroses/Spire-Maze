using System;
using CrazyGames;

namespace CodeBase.SDK.ADS
{
    public class CrazyGamesAD : IADProvider
    {
        public void ShowRewardAd(Action onCompleteCallback = null, Action<string> onDenyCallback = null)
        {
            throw new NotImplementedException();
        }

        public void ShowInterstitialAd(Action onCompleteCallback = null, Action<string> onDenyCallback = null)
        {
            throw new NotImplementedException();
        }
    }
}