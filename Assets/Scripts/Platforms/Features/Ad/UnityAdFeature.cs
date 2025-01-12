using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Platforms.Ad
{
    public class UnityAdFeature : IAdFeature
    {
        public event Action FullscreenOpenEvent;
        public event Action FullscreenCloseEvent;
        public event Action FullscreenErrorEvent;
        public event Action RewardedOpenEvent;
        public event Action RewardedCloseEvent;
        public event Action<string> RewardedSuccessEvent;
        public event Action RewardedCloseError;

        public void ShowFullscreen()
        {
            FullscreenOpenEvent?.Invoke();
            FullscreenCloseEvent?.Invoke();
            ShowDebugMessage();
        }

        public void ShowRewarded(string id)
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

#if UNITY_2023_1_OR_NEWER
        public async Awaitable ShowFullscreenAwaitable()
        {
            await Awaitable.NextFrameAsync();
        }

        public async Awaitable<bool> ShowRewardedAwaitable(string id)
        {
            await Awaitable.NextFrameAsync();
            return true;
        }
#endif
    }
}