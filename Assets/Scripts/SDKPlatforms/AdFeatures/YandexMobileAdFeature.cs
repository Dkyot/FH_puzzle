using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

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
        
        private RewardedAdLoader _rewardedAdLoader;
        private RewardedAd _rewardedAd;
        private int _currentRewardId;
        private string _adUnitId = "demo-rewarded-yandex";
        private bool _adRewarded;
        private bool _adClosed;
        private bool _adError;

        public YandexMobileAdFeature(string adUnitId)
        {
            if (!string.IsNullOrEmpty(adUnitId))
            {
                _adUnitId = adUnitId;
            }
            SetupLoader();
            RequestRewardedAd();
        }
        
        public void ShowFullscreen()
        {
            
        }

        public void ShowRewarded(int id)
        {
            _currentRewardId = id;
            _rewardedAd?.Show();
        }

#if UNITY_2023
        public async Awaitable ShowFullscreenAwaitable()
        {
            await Awaitable.NextFrameAsync();
        }

        public async Awaitable<bool> ShowRewardedAwaitable(int id)
        {
            _adClosed = _adError = _adRewarded = false;

            ShowRewarded(id);

            while (!_adRewarded && !_adError && !_adClosed)
            {
                await Awaitable.NextFrameAsync();
            }

            return _adRewarded;
        }
#endif
        
        private void SetupLoader()
        {
            _rewardedAdLoader = new RewardedAdLoader();
            _rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
            _rewardedAdLoader.OnAdFailedToLoad += OnAdFailedToLoad;
        }
        
        private void RequestRewardedAd()
        {
            var adRequestConfiguration = new AdRequestConfiguration.Builder(_adUnitId).Build();
            _rewardedAdLoader.LoadAd(adRequestConfiguration);
        }

        private void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
        {
            _rewardedAd = args.RewardedAd;
            _rewardedAd.OnAdShown += OnAdShown;
            _rewardedAd.OnAdFailedToShow += OnAdFailedToShow;
            _rewardedAd.OnAdDismissed += OnAdDismissed;
            _rewardedAd.OnRewarded += OnRewarded;
        }

        private void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log($"Ad {args.AdUnitId} failed for to load with {args.Message}");
        }

        private void OnAdDismissed(object sender, EventArgs args)
        {
            _adClosed = true;
            DestroyRewardedAd();
            RequestRewardedAd();
        }

        private void OnAdFailedToShow(object sender, AdFailureEventArgs args)
        {
            _adError = true;
            DestroyRewardedAd();
            RequestRewardedAd();
            RewardedCloseError?.Invoke();
        }

        private void OnAdShown(object sender, EventArgs args)
        {
            RewardedOpenEvent?.Invoke();
        }

        private void OnRewarded(object sender, Reward args)
        {
            _adRewarded = true;
            RewardedSuccessEvent?.Invoke(_currentRewardId);
            RewardedCloseEvent?.Invoke();
        }

        private void DestroyRewardedAd()
        {
            if (_rewardedAd == null) return;
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }
}