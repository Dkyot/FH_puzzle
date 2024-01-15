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
            Debug.Log(JsonConvert.SerializeObject(SaveInfo));
            if (YandexGame.savesData.isFirstSession)
            {
                SaveInfo.Language = YandexGame.EnvironmentData.language;
            }

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