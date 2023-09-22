using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class RewardedAdManager : MonoBehaviour
    {
        public static RewardedAdManager Instance { get; private set; }

        private bool _awarded;
        private bool _adClosed;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async Awaitable<bool> ShowAdAwaitable()
        {
            _adClosed = false;
            _awarded = false;
            YandexGamesManager.ShowRewardedAdv(gameObject, nameof(AdvCallback));
            while (!_adClosed || !_awarded)
            {
                await Awaitable.NextFrameAsync();
            }

            return _awarded;
        }

        public async Awaitable WaitingAdClose()
        {
            while (!_adClosed)
            {
                await Awaitable.NextFrameAsync();
            }
        }

        public void AdvCallback(int result)
        {
            switch (result)
            {
                case 0:
                    PauseManager.Instance.PauseGame();
                    break;
                case 1:
                    _awarded = true;
                    break;
                case 2:
                    PauseManager.Instance.ResumeGame();
                    _adClosed = true;
                    break;
            }
        }
    }
}