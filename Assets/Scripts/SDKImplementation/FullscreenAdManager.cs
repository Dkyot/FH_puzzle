using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class FullscreenAdManager : MonoBehaviour
    {
        public static FullscreenAdManager Instance { get; private set; }

        [SerializeField] private bool showStartup;
        [SerializeField] private int delayStartup;
        [SerializeField] private int delaySeconds;

        private static DateTime _adsTime;
        private static readonly DateTime StartTime;

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

        private void Start()
        {
            if (showStartup)
            {
                ShowAdv();
            }
        }

        public void ShowAdv()
        {
            if (DateTime.UtcNow - StartTime <= TimeSpan.FromSeconds(delayStartup)) return;
            if (DateTime.UtcNow - _adsTime > TimeSpan.FromSeconds(delaySeconds))
            {
                YandexGamesManager.ShowSplashAdv(gameObject.name, nameof(AdvCallback));
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
                    PauseManager.Instance.ResumeGame();
                    _adsTime = DateTime.UtcNow;
                    break;
            }
        }
    }
}