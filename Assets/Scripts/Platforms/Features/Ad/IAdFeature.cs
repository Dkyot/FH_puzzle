using System;
using UnityEngine;

namespace Platforms.Ad
{
    public interface IAdFeature
    {
        event Action FullscreenOpenEvent;
        event Action FullscreenCloseEvent;
        event Action FullscreenErrorEvent;

        event Action RewardedOpenEvent;
        event Action RewardedCloseEvent;
        event Action<string> RewardedSuccessEvent;
        event Action RewardedCloseError;
        
        void ShowFullscreen();
        void ShowRewarded(string id);

#if UNITY_2023_1_OR_NEWER
        Awaitable ShowFullscreenAwaitable();
        Awaitable<bool> ShowRewardedAwaitable(string id);
#endif
    }
}