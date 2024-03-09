using System;
using FH.Cards;
using FH.UI.Views.GameUI;
using UnityEngine;
using FH.Inputs;
using PlatformsSdk.Main;
using PlatformsSdk.MetrikaFeatures;

namespace FH.Level {
    public sealed class LevelAdController : MonoBehaviour {
        [SerializeField] private int _findPairFreeUsage = 3;
        [SerializeField] private int _peekFreeUsage = 1;
        
        [SerializeField] private int _findPairAdUsage = 5;
        [SerializeField] private int _peekAdUsage = 3;

        [Header("Scene References")]
        [SerializeField] private LevelSceneController _levelController;
        [SerializeField] private GameUIViewController _gameUIController;
        [SerializeField] private CardManager _cardManager;

        private bool _findPairIsRunning = false;
        private bool _peekIsRunning = false;

        private int _currentPairUsage;
        private int _currentPeekUsage;

        public async void TryUseFindPair() {
            if (_findPairIsRunning)
                return;

            _findPairIsRunning = true;
            bool shouldUse = false;

            if (_currentPairUsage <= 0) {
                if (await ShowAd()) {
                    shouldUse = true;
                    _currentPairUsage = _findPairAdUsage > 0 ? _findPairAdUsage - 1 : 1;
                    MetrikaFeatures.Instance.SendEvent(MetrikaEventEnum.PairReceived);
                }
            }
            else {
                shouldUse = true;
                _currentPairUsage--;
            }

            _gameUIController.SetFindPairUsageCount(_currentPairUsage);

            if (shouldUse)
                _cardManager.FindPair();

            _findPairIsRunning = false;

            if (_currentPairUsage == 0)
            {
                MetrikaFeatures.Instance.SendEvent(MetrikaEventEnum.PairAllUsed);
            }
        }

        public async void TryUsePeek() {
            if (_peekIsRunning)
                return;

            _peekIsRunning = true;
            bool shouldUse = false;

            if (_currentPeekUsage <= 0) {
                if (await ShowAd())
                {
                    _currentPeekUsage += _peekAdUsage;
                    MetrikaFeatures.Instance.SendEvent(MetrikaEventEnum.EyeReceived);
                }
            }
            else {
                shouldUse = true;
                _currentPeekUsage--;
            }

            _gameUIController.SetPeekUsegeCount(_currentPeekUsage);

            if (shouldUse) {
                _levelController.FreezeGame();
                await _cardManager.WaveTip();
                _levelController.UnFreezeGame();
            }

            _peekIsRunning = false;
            
            if (_currentPeekUsage == 0)
            {
                MetrikaFeatures.Instance.SendEvent(MetrikaEventEnum.EyeAllUsed);
            }
        }

        public void ResetTips()
        {
            _currentPairUsage = _findPairFreeUsage;
            _currentPeekUsage = _peekFreeUsage;
            
            _gameUIController.SetFindPairUsageCount(_currentPairUsage);
            _gameUIController.SetPeekUsegeCount(_currentPeekUsage);
        }

        private async Awaitable<bool> ShowAd() {
            _levelController.FreezeGame();
            bool adResult = await PlatformFeatures.Ad.ShowRewardedAwaitable(1);;
            _levelController.UnFreezeGame();
            return adResult;
        }
        
        private void Start()
        {
            ResetTips();
            
            _gameUIController.SetAdFinPairdBonus(_findPairAdUsage);
            _gameUIController.SetAdPeekBonus(_peekAdUsage);
        }
    }
}
