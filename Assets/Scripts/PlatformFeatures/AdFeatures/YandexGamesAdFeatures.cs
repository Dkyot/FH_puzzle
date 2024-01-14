using System;
using UnityEngine;
using YG;

namespace PlatformFeatures
{
    public class YandexGamesAdFeatures : AdFeaturesBase
    {
        [SerializeField] private YandexGame yandexGamePrefab;

        public override event Action FullscreenOpenEvent;
        public override event Action FullscreenCloseEvent;
        public override event Action RewardedOpenEvent;
        public override event Action RewardedCloseEvent;
        public override event Action<int> RewardedSuccessEvent;
        public override event Action RewardedCloseError;

        private void OnEnable()
        {
            YandexGame.OpenFullAdEvent += FullscreenOpenEvent;
            YandexGame.OpenFullAdEvent += FullscreenCloseEvent;
            YandexGame.OpenVideoEvent += RewardedOpenEvent;
            YandexGame.CloseVideoEvent += RewardedCloseEvent;
            YandexGame.RewardVideoEvent += RewardedSuccessEvent;
            YandexGame.ErrorVideoEvent += RewardedCloseError;
        }
        
        private void OnDisable()
        {
            YandexGame.OpenFullAdEvent -= FullscreenOpenEvent;
            YandexGame.OpenFullAdEvent -= FullscreenCloseEvent;
            YandexGame.OpenVideoEvent -= RewardedOpenEvent;
            YandexGame.CloseVideoEvent -= RewardedCloseEvent;
            YandexGame.RewardVideoEvent -= RewardedSuccessEvent;
            YandexGame.ErrorVideoEvent -= RewardedCloseError;
        }
        
        protected override void Init()
        {
            if (yandexGamePrefab == null)
            {
                Debug.LogError("No Yandex plugin prefab");
            }
        }

        public override void ShowFullscreen()
        {
            YandexGame.FullscreenShow();
        }

        public override void ShowRewarded(int id)
        {
            YandexGame.RewVideoShow(id);
        }

#if UNITY_2023
        private bool _rewardedSuccess;
        private int _currentRewardedId;
        private bool _adOpened;

        public override async Awaitable ShowFullscreenAwaitable()
        {
            if (YandexGame.nowAdsShow) return;
            _adOpened = false;

            YandexGame.OpenFullAdEvent += OnOpenAdEvent;
            YandexGame.FullscreenShow();

            while (!_adOpened || YandexGame.nowFullAd)
            {
                await Awaitable.NextFrameAsync();
            }
            
            YandexGame.OpenFullAdEvent -= OnOpenAdEvent;
        }

        public override async Awaitable<bool> ShowRewardedAwaitable(int id)
        {
            if (YandexGame.nowAdsShow)
            {
                return false;
            }

            _rewardedSuccess = false;
            _currentRewardedId = id;
            _adOpened = false;

            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent += OnErrorAdEvent;
            YandexGame.OpenVideoEvent += OnOpenAdEvent;
            YandexGame.RewVideoShow(_currentRewardedId);

            while (!_rewardedSuccess || !_adOpened || YandexGame.nowVideoAd)
            {
                await Awaitable.NextFrameAsync();
            }

            YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent -= OnErrorAdEvent;
            YandexGame.OpenVideoEvent -= OnOpenAdEvent;

            return _rewardedSuccess;
        }

        private void OnOpenAdEvent()
        {
            _adOpened = true;
        }

        private void OnRewardVideoEvent(int id)
        {
            if (_currentRewardedId == id)
            {
                _rewardedSuccess = true;
            }
        }

        private void OnErrorAdEvent()
        {
            Debug.LogError("Error ad show");
            _rewardedSuccess = false;
        }
#endif
    }
}