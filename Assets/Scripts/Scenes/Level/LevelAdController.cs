using FH.UI.Views.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Level {
    public sealed class LevelAdController : MonoBehaviour {
        [SerializeField] private int _findPairFreeUsage = 3;
        [SerializeField] private int _peekFreeUsage = 0;

        [Header("Scene References")]
        [SerializeField] private LevelSceneController _levelController;
        [SerializeField] private GameUIViewController _gameUIViewController;

        public async void TryUseFindPair() {
            if (_findPairFreeUsage <= 0) {
                _levelController.FreezeGame();
                // Todo add Ad logic
                // await ShowAd();
                _levelController.UnFreezeGame();
            }
            else {
                _findPairFreeUsage--;
                _gameUIViewController.SetFindPairUsageCount(_findPairFreeUsage);
            }

            // Todo add find pair logic
        }

        public async void TryUsePeek() {
            if (_peekFreeUsage <= 0) {
                _levelController.FreezeGame();
                // Todo add Ad logic
                // await ShowAd();
                _levelController.UnFreezeGame();
            }
            else {
                _peekFreeUsage--;
                _gameUIViewController.SetFindPairUsageCount(_peekFreeUsage);
            }

            // Todo add peek logic
        }

        private void Start() {
            _gameUIViewController.SetFindPairUsageCount(_findPairFreeUsage);
            _gameUIViewController.SetPeekUsegeCount(_peekFreeUsage);
        }
    }
}
