using UnityEngine;

namespace CrazyGames
{
    class CrazySDKInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
#if !CRAZY_GAMES
            return;
#endif

            CrazySDK.ResetDomain();
            CrazyAds.ResetDomain();
            CrazyEvents.ResetDomain();
            CrazyUser.ResetDomain();
#if UNITY_WEBGL
            SiteLock.Check();
            var sdk = CrazySDK.Instance; // this will create the SDK GameObject
#endif
        }
    }
}