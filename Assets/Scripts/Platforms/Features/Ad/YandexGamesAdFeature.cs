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
        public event Action<string> RewardedSuccessEvent;
        public event Action RewardedCloseError;
        
        private bool _callbackInit;
        
        public void InitCallbacks()
        {
            if (_callbackInit) return;

            YG2.onOpenInterAdv += FullscreenOpenEvent;
            YG2.onCloseInterAdv += FullscreenCloseEvent;
            YG2.onErrorInterAdv += FullscreenErrorEvent;
            YG2.onOpenRewardedAdv += RewardedOpenEvent;
            YG2.onCloseRewaededAdv += RewardedCloseEvent;
            YG2.onErrorRewardedAdv += RewardedCloseError;
            YG2.onRewardAdv += RewardedSuccessEvent;

            _callbackInit = true;
        }

        ~YandexGamesAdFeature()
        {
            YG2.onOpenInterAdv -= FullscreenOpenEvent;
            YG2.onCloseInterAdv -= FullscreenCloseEvent;
            YG2.onErrorInterAdv -= FullscreenErrorEvent;
            YG2.onOpenRewardedAdv -= RewardedOpenEvent;
            YG2.onCloseRewaededAdv -= RewardedCloseEvent;
            YG2.onErrorRewardedAdv -= RewardedCloseError;
            YG2.onRewardAdv -= RewardedSuccessEvent;
        }

        public void ShowFullscreen()
        {
            YG2.InterstitialAdvShow();
        }

        public void ShowRewarded(string id)
        {
            YG2.RewardedAdvShow(id);
        }

#if UNITY_2023_1_OR_NEWER
        private string _currentRewardedId;
        private bool _adRewarded;
        private bool _adClosed;
        private bool _adError;

        public async Awaitable ShowFullscreenAwaitable()
        {
            if (YG2.nowAdsShow || !YG2.isTimerAdvCompleted) return;
            
            _adClosed = _adError = _adRewarded = false;
            
            YG2.onCloseInterAdv += OnCloseAdEvent;
            YG2.onErrorInterAdv += OnErrorAdEvent;
            YG2.InterstitialAdvShow();
            
            while (!_adError && !_adClosed)
            {
                await Awaitable.NextFrameAsync();
            }
            
            YG2.onCloseInterAdv -= FullscreenCloseEvent;
            YG2.onErrorInterAdv -= FullscreenErrorEvent;
        }

        public async Awaitable<bool> ShowRewardedAwaitable(string id)
        {
            if (YG2.nowAdsShow)
            {
                return false;
            }
            
            _adClosed = _adError = _adRewarded = false;
            _currentRewardedId = id;
            
            YG2.onRewardAdv += OnRewardVideoEvent;
            YG2.onErrorRewardedAdv += OnErrorAdEvent;
            YG2.onCloseRewaededAdv += OnCloseAdEvent;
            YG2.RewardedAdvShow(_currentRewardedId);
            
            while (!_adRewarded && !_adError && !_adClosed)
            {
                await Awaitable.NextFrameAsync();
            }
            
            if (YG2.infoYG.RewardedAdv.rewardedAfterClosing)
            {
                while (!_adClosed)
                {
                    await Awaitable.NextFrameAsync();
                }
            }
            
            YG2.onRewardAdv -= OnRewardVideoEvent;
            YG2.onErrorRewardedAdv -= OnErrorAdEvent;
            YG2.onCloseRewaededAdv -= OnCloseAdEvent;

            return _adRewarded;
        }

        private void OnCloseAdEvent()
        {
            _adClosed = true;
        }

        private void OnRewardVideoEvent(string id)
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