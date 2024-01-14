using FH.Cards;
using FH.UI.Views.GameUI;
using UnityEngine;
using FH.Inputs;
using PlatformFeatures;
using PlatformFeatures.AdFeatures;

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
                    //todo: YandexMetrika.PairReceived();
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
                //todo: YandexMetrika.PairAllUsed();
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
                    //todo: YandexMetrika.EyeReceived();
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
                //todo: YandexMetrika.EyeAllUsed();
            }
        }

        private async Awaitable<bool> ShowAd() {
            _levelController.FreezeGame();
            bool adResult = await AdFeatures.Instance.ShowRewardedAwaitable(1);;
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
