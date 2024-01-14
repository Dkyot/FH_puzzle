using System;
using UnityEngine;
using YG;

namespace PlatformFeatures.SaveFeatures
{
    public class YandexGamesSaveFeatures : SaveFeatures
    {
        public override event Action DataLoadedEvent;

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

        protected override void Init()
        {
            LoadData();
        }

        public override void LoadData()
        {
            if (!YandexGame.SDKEnabled) return;
            SaveInfo = YandexGame.savesData.saveInfo;
            if (YandexGame.savesData.isFirstSession)
            {
                SaveInfo.Language = YandexGame.savesData.language;
            }

            DataLoadedEvent?.Invoke();
        }

        public override void SaveData()
        {
            YandexGame.savesData.saveInfo = SaveInfo;
            YandexGame.SaveProgress();
        }

#if UNITY_2023
        private bool _dataLoaded;
        public override async Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds)
        {
            if (YandexGame.SDKEnabled && YandexGame.savesData != null) return true;
            YandexGame.LoadProgress();

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