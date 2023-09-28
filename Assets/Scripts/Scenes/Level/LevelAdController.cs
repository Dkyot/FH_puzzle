using FH.Cards;
using FH.UI.Views.GameUI;
using System.Collections;
using System.Collections.Generic;
using SkibidiRunner.Managers;
using UnityEngine;

namespace FH.Level {
    public sealed class LevelAdController : MonoBehaviour {
        [SerializeField] private int _findPairFreeUsage = 3;
        [SerializeField] private int _peekFreeUsage = 0;

        [Header("Scene References")]
        [SerializeField] private LevelSceneController _levelController;
        [SerializeField] private GameUIViewController _gameUIViewController;
        [SerializeField] private CardManager _cardManager;

        public async void TryUseFindPair() {
            if (_findPairFreeUsage <= 0) {
                _levelController.FreezeGame();

                if (await ShowAd()) {
                    _findPairFreeUsage += 2;
                }

                _levelController.UnFreezeGame();
            }
            else {
                _findPairFreeUsage--;
            }

            _gameUIViewController.SetFindPairUsageCount(_findPairFreeUsage);
            _cardManager.FindPair();
        }

        public async void TryUsePeek() {
            if (_peekFreeUsage <= 0) {
                _levelController.FreezeGame();

                if (await ShowAd()) {
                    Debug.Log("НАГРАДА");
                }

                _levelController.UnFreezeGame();
            }
            else {
                _peekFreeUsage--;
            }

            _gameUIViewController.SetFindPairUsageCount(_peekFreeUsage);
            await _cardManager.WaveTip();
        }

        private async Awaitable<bool> ShowAd() {
            var adManager = RewardedAdManager.Instance;
            if (adManager == null)
                return true;

            var adResult = await adManager.ShowAdAwaitable();
            await adManager.WaitingAdClose();
            return adResult;
        }

        private void Start() {
            _gameUIViewController.SetFindPairUsageCount(_findPairFreeUsage);
            _gameUIViewController.SetPeekUsegeCount(_peekFreeUsage);
        }
    }
}
