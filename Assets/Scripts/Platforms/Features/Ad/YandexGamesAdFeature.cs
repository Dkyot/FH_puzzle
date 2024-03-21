using System;
using UnityEngine;
using YG;

namespace Platforms.Ad
{
    public class YandexGamesAdFeature : IAdFeature
    {
        public event Action FullscreenOpenEvent;
        public event Action FullscreenCloseEvent;
        public event Action FullscreenErrorEvent;
        public event Action RewardedOpenEvent;
        public event Action RewardedCloseEvent;
        public event Action<int> RewardedSuccessEvent;
        public event Action RewardedCloseError;
        
        private readonly InfoYG _infoYg;
        private bool _callbackInit;
        
        public YandexGamesAdFeature(InfoYG infoYg)
        {
            _infoYg = infoYg;
        }

        public void InitCallbacks()
        {
            if(_callbackInit) return;
            
            YandexGame.OpenFullAdEvent += FullscreenOpenEvent;
            YandexGame.CloseFullAdEvent += FullscreenCloseEvent;
            YandexGame.ErrorFullAdEvent += FullscreenErrorEvent;
            YandexGame.OpenVideoEvent += RewardedOpenEvent;
            YandexGame.CloseVideoEvent += RewardedCloseEvent;
            YandexGame.RewardVideoEvent += RewardedSuccessEvent;
            YandexGame.ErrorVideoEvent += RewardedCloseError;

            _callbackInit = true;
        }

        ~YandexGamesAdFeature()
        {
            YandexGame.OpenFullAdEvent -= FullscreenOpenEvent;
            YandexGame.OpenFullAdEvent -= FullscreenCloseEvent;
            YandexGame.OpenVideoEvent -= RewardedOpenEvent;
            YandexGame.CloseVideoEvent -= RewardedCloseEvent;
            YandexGame.RewardVideoEvent -= RewardedSuccessEvent;
            YandexGame.ErrorVideoEvent -= RewardedCloseError;
        }

        public void ShowFullscreen()
        {
            YandexGame.FullscreenShow();
        }

        public void ShowRewarded(int id)
        {
            YandexGame.RewVideoShow(id);
        }

#if UNITY_2023
        private int _currentRewardedId;
        private bool _adRewarded;
        private bool _adClosed;
        private bool _adError;

        public async Awaitable ShowFullscreenAwaitable()
        {
            if (YandexGame.nowAdsShow || YandexGame.timerShowAd < _infoYg.fullscreenAdInterval) return;

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

        public async Awaitable<bool> ShowRewardedAwaitable(int id)
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

            if (_infoYg.rewardedAfterClosing)
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