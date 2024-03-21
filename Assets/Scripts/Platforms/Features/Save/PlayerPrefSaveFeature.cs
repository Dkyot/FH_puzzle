using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Platforms.Save
{
    public class PlayerPrefSaveFeature : ISaveFeature
    {
        public SaveInfo SaveInfo { get; private set; } = new();

        public event Action DataLoadedEvent;

        private readonly string _saveKey;

        public PlayerPrefSaveFeature(string saveKey)
        {
            _saveKey = saveKey;
            LoadData();
        }

        public void LoadData()
        {
            string json = PlayerPrefs.GetString(_saveKey);
            var loadedData = JsonConvert.DeserializeObject<SaveInfo>(json);
            if (loadedData != null)
            {
                SaveInfo = loadedData;
            }
            DataLoadedEvent?.Invoke();
        }

        public void SaveData()
        {
            string json = JsonConvert.SerializeObject(SaveInfo);
            PlayerPrefs.SetString(_saveKey, json);
            PlayerPrefs.Save();
        }

#if UNITY_2023
        public async Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds)
        {
            LoadData();
            await Awaitable.NextFrameAsync();
            return true;
        }
#endif
    }
}