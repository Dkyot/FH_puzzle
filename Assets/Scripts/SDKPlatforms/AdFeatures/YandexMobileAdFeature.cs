using System;
using UnityEngine;

namespace PlatformsSdk.AdFeatures
{
    public class YandexMobileAdFeature : IAdFeature
    {
        public event Action FullscreenOpenEvent;
        public event Action FullscreenCloseEvent;
        public event Action FullscreenErrorEvent;
        public event Action RewardedOpenEvent;
        public event Action RewardedCloseEvent;
        public event Action<int> RewardedSuccessEvent;
        public event Action RewardedCloseError;
        
        private string _adUnitId = "demo-rewarded-yandex";

        public YandexMobileAdFeature(string adUnitId)
        {
            if (!string.IsNullOrEmpty(adUnitId))
            {
                _adUnitId = adUnitId;
            }
            
        }
        
        public void ShowFullscreen()
        {
            
        }

        public void ShowRewarded(int id)
        {
            throw new NotImplementedException();
        }

#if UNITY_2023
        public Awaitable ShowFullscreenAwaitable()
        {
            throw new NotImplementedException();
        }

        public Awaitable<bool> ShowRewardedAwaitable(int id)
        {
            throw new NotImplementedException();
        }
#endif
    }
}