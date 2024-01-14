using System;
using UnityEngine;

namespace PlatformFeatures
{
    public abstract class AdFeaturesBase : MonoBehaviour
    {
        public static AdFeaturesBase Instance { get; private set;}

        protected virtual void Awake()
        {
            Init();
            Instance = this;
        }

        protected abstract void Init();

        public abstract event Action FullscreenOpenEvent;
        public abstract event Action FullscreenCloseEvent;
        
        public abstract event Action RewardedOpenEvent;
        public abstract event Action RewardedCloseEvent;
        public abstract event Action<int> RewardedSuccessEvent;
        public abstract event Action RewardedCloseError;
        
        public abstract void ShowFullscreen();
        public abstract void ShowRewarded(int id);

#if UNITY_2023
        public abstract Awaitable ShowFullscreenAwaitable();
        public abstract Awaitable<bool> ShowRewardedAwaitable(int id);
#endif
    }
}