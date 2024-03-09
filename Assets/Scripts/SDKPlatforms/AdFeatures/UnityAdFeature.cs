using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PlatformsSdk.AdFeatures
{
    public class UnityAdFeature : IAdFeature
    {
        public event Action FullscreenOpenEvent;
        public event Action FullscreenCloseEvent;
        public event Action FullscreenErrorEvent;
        public event Action RewardedOpenEvent;
        public event Action RewardedCloseEvent;
        public event Action<int> RewardedSuccessEvent;
        public event Action RewardedCloseError;

        public void ShowFullscreen()
        {
            FullscreenOpenEvent?.Invoke();
            FullscreenCloseEvent?.Invoke();
            ShowDebugMessage();
        }

        public void ShowRewarded(int id)
        {
            RewardedOpenEvent?.Invoke();
            RewardedSuccessEvent?.Invoke(id);
            RewardedCloseEvent?.Invoke();
            ShowDebugMessage();
        }


        private static void ShowDebugMessage([CallerMemberName] string functionName = "")
        {
            Debug.Log($"Debug {functionName}");
        }

#if UNITY_2023
        public async Awaitable ShowFullscreenAwaitable()
        {
            await Awaitable.NextFrameAsync();
        }

        public async Awaitable<bool> ShowRewardedAwaitable(int id)
        {
            await Awaitable.NextFrameAsync();
            return true;
        }
#endif
    }
}