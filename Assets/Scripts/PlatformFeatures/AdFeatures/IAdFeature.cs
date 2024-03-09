using System;
using UnityEngine;

namespace PlatformFeatures.AdFeatures
{
    public interface IAdFeature
    {
        event Action FullscreenOpenEvent;
        event Action FullscreenCloseEvent;
        event Action FullscreenErrorEvent;

        event Action RewardedOpenEvent;
        event Action RewardedCloseEvent;
        event Action<int> RewardedSuccessEvent;
        event Action RewardedCloseError;
        
        void ShowFullscreen();
        void ShowRewarded(int id);

#if UNITY_2023
        Awaitable ShowFullscreenAwaitable();
        Awaitable<bool> ShowRewardedAwaitable(int id);
#endif
    }
}