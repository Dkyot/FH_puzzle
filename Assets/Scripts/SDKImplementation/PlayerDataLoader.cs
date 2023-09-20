using System;
using System.Threading.Tasks;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class PlayerDataLoader : MonoBehaviour
    {
        public static PlayerDataLoader Instance { get; private set; }

        private bool _dataLoaded;

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

        public void TryStartLoad()
        {
            if (LocalYandexData.Instance.YandexDataLoaded) return;
            YandexGamesManager.LoadPlayerData(gameObject, nameof(OnPlayerDataReceived));
        }

        public async Awaitable<bool> TryLoadAwaitable(int waitingTimeSeconds)
        {
            Debug.Log("START LOAD PLAYER DATA");
            if (LocalYandexData.Instance.YandexDataLoaded) return true;
            _dataLoaded = false;
            YandexGamesManager.LoadPlayerData(gameObject, nameof(OnPlayerDataReceived));
            var time = DateTime.UtcNow;
            var timeout = TimeSpan.FromSeconds(waitingTimeSeconds);
            while (!_dataLoaded && DateTime.UtcNow - time < timeout)
            {
                await Awaitable.FixedUpdateAsync();
            }

            return _dataLoaded;
        }

        public void OnPlayerDataReceived(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Failed to load player data");
                TryStartLoad();
            }
            else
            {
                if (json != "DEBUG")
                {
                    LocalYandexData.Instance.SetPlayerData(JsonUtility.FromJson<SaveInfo>(json));
                }

                Debug.Log("Data loaded");
                _dataLoaded = true;
            }
        }
    }
}