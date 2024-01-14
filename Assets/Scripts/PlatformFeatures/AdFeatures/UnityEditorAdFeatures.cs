using System;
using UnityEngine;

namespace PlatformFeatures
{
    public class UnityEditorAdFeatures : AdFeaturesBase
    {
        public override event Action FullscreenOpenEvent;
        public override event Action FullscreenCloseEvent;
        public override event Action RewardedOpenEvent;
        public override event Action RewardedCloseEvent;
        public override event Action<int> RewardedSuccessEvent;
        public override event Action RewardedCloseError;

        protected override void Init()
        {
        }

        public override void ShowFullscreen()
        {
        }

        public override void ShowRewarded(int id)
        {
            RewardedOpenEvent?.Invoke();
            RewardedSuccessEvent?.Invoke(id);
            RewardedCloseEvent?.Invoke();
        }

#if UNITY_2023
        public override async Awaitable ShowFullscreenAwaitable()
        {
            await Awaitable.NextFrameAsync();
        }

        public override async Awaitable<bool> ShowRewardedAwaitable(int id)
        {
            await Awaitable.NextFrameAsync();
            return true;
        }
#endif
    }
}