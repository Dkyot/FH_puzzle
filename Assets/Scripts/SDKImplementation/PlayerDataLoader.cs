using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class PlayerDataLoader : MonoBehaviour
    {
        [SerializeField] private bool tryLoadOnInit;
        [SerializeField] private float timeoutTryLoadSeconds;
        [SerializeField] private int maxNumberAttempts;
        
        public static PlayerDataLoader Instance { get; private set; }

        private bool _dataLoaded;
        private int _numberAttempts;
        private bool _loadingStarted;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                if (tryLoadOnInit)
                {
                    TryStartLoad();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void TryStartLoad()
        {
            if (LocalYandexData.Instance.YandexDataLoaded) return;
            YandexGamesManager.LoadPlayerData(gameObject, nameof(OnPlayerDataReceived));
            _numberAttempts++;
            _loadingStarted = true;
        }

        public async Awaitable<bool> TryLoadAwaitable(int waitingTimeSeconds)
        {
            if (LocalYandexData.Instance.YandexDataLoaded) return true;
            if (!_loadingStarted)
            {
                _dataLoaded = false;
                _numberAttempts = 1;
                YandexGamesManager.LoadPlayerData(gameObject, nameof(OnPlayerDataReceived));
                _loadingStarted = true;
            }
            
            var time = DateTime.UtcNow;
            var timeout = TimeSpan.FromSeconds(waitingTimeSeconds);
            while (!_dataLoaded && DateTime.UtcNow - time < timeout)
            {
                await Awaitable.NextFrameAsync();
            }

            return _dataLoaded;
        }

        public async Awaitable OnPlayerDataReceived(string json)
        {
            _loadingStarted = false;
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Failed to load player data");
                if(_numberAttempts >= maxNumberAttempts) return;
                await Awaitable.WaitForSecondsAsync(timeoutTryLoadSeconds);
                TryStartLoad();
            }
            else
            {
                if (json != "DEBUG" && !LocalYandexData.Instance.YandexDataLoaded)
                {
                    LocalYandexData.Instance.SetPlayerData(JsonConvert.DeserializeObject<SaveInfo>(json));
                }

                Debug.Log("Yandex data successfully loaded");
                _dataLoaded = true;
            }
        }
    }
}