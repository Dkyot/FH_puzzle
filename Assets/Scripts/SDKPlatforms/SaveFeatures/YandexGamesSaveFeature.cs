using System;
using Newtonsoft.Json;
using UnityEngine;
using YG;

namespace PlatformsSdk.SaveFeatures
{
    public class YandexGamesSaveFeature : ISaveFeature
    {
        public SaveInfo SaveInfo { get; private set; } = new ();
        
        public event Action DataLoadedEvent;

        private bool _dataLoaded;
        private bool _callbackInit;

        public YandexGamesSaveFeature()
        {
            if (YandexGame.SDKEnabled)
            {
                LoadData();
            }
        }
        
        public void InitCallbacks()
        {
            if(_callbackInit) return;
            
            YandexGame.GetDataEvent += DataLoadedEvent;
            DataLoadedEvent += LoadData;
            _callbackInit = true;
        }
        
        ~YandexGamesSaveFeature()
        {
            YandexGame.GetDataEvent -= DataLoadedEvent;
            DataLoadedEvent -= LoadData;
        }

        public void LoadData()
        {
            if (_dataLoaded) return;
            SaveInfo = YandexGame.savesData.saveInfo;
            if (string.IsNullOrEmpty(SaveInfo.Language))
            {
                YandexGame.LanguageRequest();
                SaveInfo.Language = YandexGame.EnvironmentData.language;
            }

            Debug.Log(JsonConvert.SerializeObject(SaveInfo));

            _dataLoaded = true;
            DataLoadedEvent?.Invoke();
        }

        public void SaveData()
        {
            SaveInfo.LastSaveTimeTicks = DateTime.UtcNow.Ticks;
            YandexGame.savesData.saveInfo = SaveInfo;
            YandexGame.SaveProgress();
        }

#if UNITY_2023
        public async Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds)
        {
            if (YandexGame.SDKEnabled)
            {
                LoadData();
                return true;
            }

            var time = DateTime.UtcNow;
            var timeout = TimeSpan.FromSeconds(waitingTimeSeconds);
            while (!_dataLoaded && DateTime.UtcNow - time < timeout)
            {
                await Awaitable.NextFrameAsync();
            }

            return _dataLoaded;
        }
#endif
    }
}