using FH.Cards;
using FH.UI.Views.GameUI;
using SkibidiRunner.Managers;
using UnityEngine;
using FH.Inputs;
using YandexSDK.Scripts;

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
                    _currentPairUsage += _findPairAdUsage;
                    YandexMetrika.PairReceived();
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
                YandexMetrika.PairAllUsed();
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
                    YandexMetrika.EyeReceived();
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
                YandexMetrika.EyeAllUsed();
            }
        }

        private async Awaitable<bool> ShowAd() {
            var adManager = RewardedAdManager.Instance;
            if (adManager == null)
                return true;

            _levelController.FreezeGame();
            var adResult = await adManager.ShowAdAwaitable();
            await adManager.WaitingAdClose();
            _levelController.UnFreezeGame();
            return adResult;
        }
        
        private void Start()
        {
            _currentPairUsage = _findPairFreeUsage;
            _currentPeekUsage = _peekFreeUsage;
            
            _gameUIController.SetFindPairUsageCount(_currentPairUsage);
            _gameUIController.SetAdFinPairdBonus(_findPairAdUsage);

            _gameUIController.SetPeekUsegeCount(_currentPeekUsage);
            _gameUIController.SetAdPeekBonus(_peekAdUsage);
        }
    }
}
