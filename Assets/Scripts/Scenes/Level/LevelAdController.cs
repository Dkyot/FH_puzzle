using FH.Cards;
using FH.UI.Views.GameUI;
using SkibidiRunner.Managers;
using UnityEngine;
using FH.Inputs;

namespace FH.Level {
    public sealed class LevelAdController : MonoBehaviour {
        [SerializeField] private int _findPairFreeUsage = 3;
        [SerializeField] private int _peekFreeUsage = 0;

        [Header("Scene References")]
        [SerializeField] private LevelSceneController _levelController;
        [SerializeField] private GameUIViewController _gameUIController;
        [SerializeField] private CardManager _cardManager;

        private bool _findPairIsRunning = false;
        private bool _peekIsRunning = false;

        public async void TryUseFindPair() {
            if (_findPairIsRunning)
                return;

            _findPairIsRunning = true;
            bool shouldUse = false;

            if (_findPairFreeUsage <= 0) {
                if (await ShowAd()) {
                    shouldUse = true;
                    _findPairFreeUsage += 2;
                }
            }
            else {
                shouldUse = true;
                _findPairFreeUsage--;
            }

            _gameUIController.SetFindPairUsageCount(_findPairFreeUsage);

            if (shouldUse)
                _cardManager.FindPair();

            _findPairIsRunning = false;
        }

        public async void TryUsePeek() {
            if (_peekIsRunning)
                return;

            _peekIsRunning = true;
            bool shouldUse;

            if (_peekFreeUsage <= 0) {
                shouldUse = await ShowAd();
            }
            else {
                shouldUse = true;
                _peekFreeUsage--;
            }

            _gameUIController.SetPeekUsegeCount(_peekFreeUsage);

            if (shouldUse) {
                _levelController.FreezeGame();
                await _cardManager.WaveTip();
                _levelController.UnFreezeGame();
            }

            _peekIsRunning = false;
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


        private void Start() {
            _gameUIController.SetFindPairUsageCount(_findPairFreeUsage);
            _gameUIController.SetPeekUsegeCount(_peekFreeUsage);
        }
    }
}
