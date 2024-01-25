using System;
using Newtonsoft.Json;
using UnityEngine;
using YG;

namespace PlatformFeatures.SaveFeatures
{
    public class YandexGamesSaveFeatures : SaveFeatures
    {
        public override event Action DataLoadedEvent;

        private bool _dataLoaded;

        private void OnEnable()
        {
            YandexGame.GetDataEvent += DataLoadedEvent;
            DataLoadedEvent += LoadData;
        }

        private void OnDisable()
        {
            YandexGame.GetDataEvent -= DataLoadedEvent;
            DataLoadedEvent -= LoadData;
        }

        private void Start()
        {
            Init();
        }

        protected override void Init()
        {
            if (!YandexGame.SDKEnabled) return;
            LoadData();
        }

        public override void LoadData()
        {
            if(_dataLoaded) return;
            SaveInfo = YandexGame.savesData.saveInfo;
            if (string.IsNullOrEmpty(SaveInfo.Language))
            {
                YandexGame.LanguageRequest();
                Debug.Log(YandexGame.savesData.language);
                Debug.Log(YandexGame.EnvironmentData.language);
                if (string.IsNullOrEmpty(YandexGame.savesData.language))
                {
                    SaveInfo.Language = YandexGame.savesData.language;
                }
                if (string.IsNullOrEmpty(YandexGame.EnvironmentData.language))
                {
                    SaveInfo.Language = YandexGame.EnvironmentData.language;
                }
                else
                {
                    SaveInfo.Language = Application.systemLanguage.ToString();
                }
                
            }
            Debug.Log(JsonConvert.SerializeObject(SaveInfo));

            _dataLoaded = true;
            DataLoadedEvent?.Invoke();
        }

        public override void SaveData()
        {
            SaveInfo.LastSaveTimeTicks = DateTime.UtcNow.Ticks;
            YandexGame.savesData.saveInfo = SaveInfo;
            YandexGame.SaveProgress();
        }

#if UNITY_2023
        public override async Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds)
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