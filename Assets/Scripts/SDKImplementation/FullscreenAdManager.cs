using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class FullscreenAdManager : MonoBehaviour
    {
        public static FullscreenAdManager Instance { get; private set; }
        
        [SerializeField] private int delayStartup;
        [SerializeField] private int delaySeconds;

        private static DateTime _adTime;
        private static readonly DateTime StartTime;

        private bool _adClosed;

        static FullscreenAdManager()
        {
            StartTime = DateTime.UtcNow;
        }

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

        public void ShowAd()
        {
            if (DateTime.UtcNow - StartTime <= TimeSpan.FromSeconds(delayStartup)) return;
            if (DateTime.UtcNow - _adTime > TimeSpan.FromSeconds(delaySeconds))
            {
                YandexGamesManager.ShowSplashAdv(gameObject, nameof(AdCallback));
            }
        }

        public async Awaitable<bool> ShowAdAwaitable()
        {
            if (DateTime.UtcNow - StartTime <= TimeSpan.FromSeconds(delayStartup)) return false;
            if (DateTime.UtcNow - _adTime <= TimeSpan.FromSeconds(delaySeconds)) return false;
            _adClosed = false;
            YandexGamesManager.ShowSplashAdv(gameObject, nameof(AdCallback));
            while (!_adClosed)
            {
                await Awaitable.NextFrameAsync();
            }

            return true;
        }

        public void AdCallback(int result)
        {
            switch (result)
            {
                case 0:
                    PauseManager.Instance.PauseGame();
                    break;
                case 1:
                    PauseManager.Instance.ResumeGame();
                    _adTime = DateTime.UtcNow;
                    _adClosed = true;
                    break;
            }
        }
    }
}