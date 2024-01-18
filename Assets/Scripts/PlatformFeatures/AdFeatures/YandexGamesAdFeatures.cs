using System;
using UnityEngine;
using YG;

namespace PlatformFeatures.AdFeatures
{
    public class YandexGamesAdFeatures : AdFeatures
    {
        [SerializeField] private InfoYG infoYg;

        public override event Action FullscreenOpenEvent;
        public override event Action FullscreenCloseEvent;
        public override event Action FullscreenErrorEvent;
        public override event Action RewardedOpenEvent;
        public override event Action RewardedCloseEvent;
        public override event Action<int> RewardedSuccessEvent;
        public override event Action RewardedCloseError;

        private void OnEnable()
        {
            YandexGame.OpenFullAdEvent += FullscreenOpenEvent;
            YandexGame.CloseFullAdEvent += FullscreenCloseEvent;
            YandexGame.ErrorFullAdEvent += FullscreenErrorEvent;
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
        private int _currentRewardedId;
        private bool _adRewarded;
        private bool _adClosed;
        private bool _adError;

        public override async Awaitable ShowFullscreenAwaitable()
        {
            if (YandexGame.nowAdsShow || YandexGame.timerShowAd < infoYg.fullscreenAdInterval) return;

            _adClosed = _adError = _adRewarded = false;

            YandexGame.ErrorFullAdEvent += OnErrorAdEvent;
            YandexGame.CloseFullAdEvent += OnCloseAdEvent;
            YandexGame.FullscreenShow();

            while (!_adError && !_adClosed)
            {
                await Awaitable.NextFrameAsync();
            }

            YandexGame.ErrorFullAdEvent -= OnErrorAdEvent;
            YandexGame.CloseFullAdEvent -= OnCloseAdEvent;
        }

        public override async Awaitable<bool> ShowRewardedAwaitable(int id)
        {
            if (YandexGame.nowAdsShow)
            {
                return false;
            }

            _adClosed = _adError = _adRewarded = false;
            _currentRewardedId = id;

            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent += OnErrorAdEvent;
            YandexGame.RewVideoShow(_currentRewardedId);

            while (!_adRewarded && !_adError && !_adClosed)
            {
                await Awaitable.NextFrameAsync();
            }

            if (infoYg.rewardedAfterClosing)
            {
                while (!_adClosed)
                {
                    await Awaitable.NextFrameAsync();
                }
            }

            YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent -= OnErrorAdEvent;

            return _adRewarded;
        }

        private void OnCloseAdEvent()
        {
            _adClosed = true;
        }

        private void OnRewardVideoEvent(int id)
        {
            if (_currentRewardedId == id)
            {
                _adRewarded = true;
            }
        }

        private void OnErrorAdEvent()
        {
            _adError = true;
        }
#endif
    }
}