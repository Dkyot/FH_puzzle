using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace Platforms.Ad
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

        private RewardedAdLoader _rewardedLoader;
        private RewardedAd _rewardedBlock;

        private InterstitialAdLoader _fullscreenLoader;
        private Interstitial _fullscreenBlock;

        private int _currentRewardId;

        private bool _adRewarded;
        private bool _adClosed;
        private bool _adError;

        private DateTime _lastFullscreenTime;
        
        private readonly string _rewardedId = "demo-rewarded-yandex";
        private readonly string _fullscreenId = "demo-interstitial-yandex";
        private readonly TimeSpan _fullscreenTimeout;

        public YandexMobileAdFeature(string rewardedId, string fullscreenId, TimeSpan fullscreenTimeout)
        {
            if (!string.IsNullOrEmpty(rewardedId))
            {
                _rewardedId = rewardedId;
            }

            if (!string.IsNullOrEmpty(fullscreenId))
            {
                _fullscreenId = fullscreenId;
            }

            _fullscreenTimeout = fullscreenTimeout;

            SetupAdLoader();
            RequestReward();
            RequestFullscreen();
        }

        public void ShowFullscreen()
        {
            if(DateTime.UtcNow - _lastFullscreenTime < _fullscreenTimeout) return;
            if (_fullscreenBlock == null)
            {
                RequestFullscreen();
            }
            else
            {
                _fullscreenBlock.Show();
                _lastFullscreenTime = DateTime.UtcNow;
            }
        }

        public void ShowRewarded(int id)
        {
            if (_rewardedBlock == null)
            {
                RequestReward();
            }
            else
            {
                _currentRewardId = id;
                _rewardedBlock?.Show();
            }
        }

#if UNITY_2023_1_OR_NEWER
        public async Awaitable ShowFullscreenAwaitable()
        {
            if(DateTime.UtcNow - _lastFullscreenTime < _fullscreenTimeout) return;
            if (_fullscreenBlock == null)
            {
                RequestFullscreen();
                return;
            }
            _adClosed = _adError = _adRewarded = false;
            ShowFullscreen();
            while (!_adRewarded && !_adError && !_adClosed)
            {
                await Awaitable.NextFrameAsync();
            }
        }

        public async Awaitable<bool> ShowRewardedAwaitable(int id)
        {
            if (_rewardedBlock == null)
            {
                RequestReward();
                return false;
            }
            _adClosed = _adError = _adRewarded = false;
            ShowRewarded(id);
            while (!_adRewarded && !_adError && !_adClosed)
            {
                await Awaitable.NextFrameAsync();
            }

            return _adRewarded;
        }
#endif

        private void SetupAdLoader()
        {
            _rewardedLoader = new RewardedAdLoader();
            _rewardedLoader.OnAdLoaded += OnRewardLoaded;
            _rewardedLoader.OnAdFailedToLoad += OnRewardFailedToLoad;

            _fullscreenLoader = new InterstitialAdLoader();
            _fullscreenLoader.OnAdLoaded += OnFullscreenLoaded;
            _fullscreenLoader.OnAdFailedToLoad += OnFullscreenFailedToLoad;
        }

        #region RewardedAd

        private void RequestReward()
        {
            var adRequestConfiguration = new AdRequestConfiguration.Builder(_rewardedId).Build();
            _rewardedLoader.LoadAd(adRequestConfiguration);
        }

        private void OnRewardLoaded(object sender, RewardedAdLoadedEventArgs args)
        {
            _rewardedBlock = args.RewardedAd;
            _rewardedBlock.OnAdShown += OnRewardOpened;
            _rewardedBlock.OnAdFailedToShow += OnRewardFailedToShow;
            _rewardedBlock.OnAdDismissed += OnRewardClosed;
            _rewardedBlock.OnRewarded += OnRewardRewarded;
        }

        private void OnRewardFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            _adError = true;
            RewardedCloseError?.Invoke();
        }

        private void OnRewardClosed(object sender, EventArgs args)
        {
            _adClosed = true;
            DestroyRewardAd();
            RequestReward();
        }

        private void OnRewardFailedToShow(object sender, AdFailureEventArgs args)
        {
            _adError = true;
            DestroyRewardAd();
            RequestReward();
            RewardedCloseError?.Invoke();
        }

        private void OnRewardOpened(object sender, EventArgs args)
        {
            RewardedOpenEvent?.Invoke();
        }

        private void OnRewardRewarded(object sender, Reward args)
        {
            _adRewarded = true;
            RewardedSuccessEvent?.Invoke(_currentRewardId);
            RewardedCloseEvent?.Invoke();
        }

        private void DestroyRewardAd()
        {
            _rewardedBlock?.Destroy();
            _rewardedBlock = null;
        }

        #endregion


        #region FullscreenAd

        private void RequestFullscreen()
        {
            var adRequestConfiguration = new AdRequestConfiguration.Builder(_fullscreenId).Build();
            _fullscreenLoader.LoadAd(adRequestConfiguration);
        }

        private void OnFullscreenLoaded(object sender, InterstitialAdLoadedEventArgs args)
        {
            _fullscreenBlock = args.Interstitial;
            _fullscreenBlock.OnAdShown += OnFullscreenOpened;
            _fullscreenBlock.OnAdFailedToShow += OnFullscreenFailedToShow;
            _fullscreenBlock.OnAdDismissed += OnFullscreenClosed;
        }

        private void OnFullscreenFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            _adError = true;
            FullscreenErrorEvent?.Invoke();
        }

        private void OnFullscreenClosed(object sender, EventArgs args)
        {
            _adClosed = true;
            FullscreenCloseEvent?.Invoke();
            DestroyFullscreen();
            RequestFullscreen();
        }

        private void OnFullscreenFailedToShow(object sender, EventArgs args)
        {
            _adError = true;
            FullscreenErrorEvent?.Invoke();
            DestroyFullscreen();
            RequestFullscreen();
        }

        private void OnFullscreenOpened(object sender, EventArgs args)
        {
            FullscreenOpenEvent?.Invoke();
        }

        private void DestroyFullscreen()
        {
            _fullscreenBlock?.Destroy();
            _fullscreenBlock = null;
        }

        #endregion

        ~YandexMobileAdFeature()
        {
            _rewardedLoader.OnAdLoaded -= OnRewardLoaded;
            _rewardedLoader.OnAdFailedToLoad -= OnRewardFailedToLoad;
            _rewardedBlock?.Destroy();
            
            _fullscreenLoader.OnAdLoaded -= OnFullscreenLoaded;
            _fullscreenLoader.OnAdFailedToLoad -= OnFullscreenFailedToLoad;
            _fullscreenBlock?.Destroy();
        }
    }
}