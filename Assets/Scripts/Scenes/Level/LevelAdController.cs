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
                if (await RewardedAdManager.Instance.ShowAdAwaitable())
                {
                    _findPairFreeUsage += 2;
                    _gameUIViewController.SetFindPairUsageCount(_findPairFreeUsage);
                }

                //Waiting for the user to switch to the game after watching the ad
                await RewardedAdManager.Instance.WaitingAdClose();
                _levelController.UnFreezeGame();
            }
            else {
                _findPairFreeUsage--;
                _gameUIViewController.SetFindPairUsageCount(_findPairFreeUsage);
            }

            _cardManager.FindPair();
        }

        public async void TryUsePeek() {
            if (_peekFreeUsage <= 0) {
                _levelController.FreezeGame();
                if (await RewardedAdManager.Instance.ShowAdAwaitable())
                {
                    //TODO: award issuance
                    Debug.Log("НАГРАДА");
                }

                //Waiting for the user to switch to the game after watching the ad
                await RewardedAdManager.Instance.WaitingAdClose();
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
