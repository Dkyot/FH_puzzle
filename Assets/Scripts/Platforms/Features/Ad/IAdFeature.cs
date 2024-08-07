﻿using System;
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
        event Action<int> RewardedSuccessEvent;
        event Action RewardedCloseError;
        
        void ShowFullscreen();
        void ShowRewarded(int id);

#if UNITY_2023_1_OR_NEWER
        Awaitable ShowFullscreenAwaitable();
        Awaitable<bool> ShowRewardedAwaitable(int id);
#endif
    }
}