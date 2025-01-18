using System;
using Newtonsoft.Json;
using Platforms.Save;
using UnityEngine;
using YG;
using YG.Insides;

namespace Platforms.Save
{
    public class YandexGamesSaveFeature : ISaveFeature
    {
        public SaveInfo SaveInfo => YG2.saves.saveInfo;
        
        public event Action DataLoadedEvent;

        private bool _loading;
        private bool _dataLoaded;
        private bool _callbackInit;

        public YandexGamesSaveFeature()
        {
            YG2.iPlatform.InitEnirData();
            YG2.onGetSDKData += OnDataLoaded;
            LoadData();
        }
        
        ~YandexGamesSaveFeature()
        {
            YG2.onGetSDKData -= OnDataLoaded;
        }

        public void LoadData()
        {
            if (_dataLoaded || _loading) return;
            YGInsides.LoadProgress();
            _loading = true;
        }

        public void SaveData()
        {
            SaveInfo.LastSaveTimeTicks = DateTime.UtcNow.Ticks;
            YG2.SaveProgress();
        }

        private void OnDataLoaded()
        {
            _dataLoaded = true;
            _loading = false;
            if (string.IsNullOrEmpty(SaveInfo.Language))
            {
                SaveInfo.Language = YG2.envir.language;
            }
            
            Debug.Log(JsonConvert.SerializeObject(SaveInfo));

            DataLoadedEvent?.Invoke();
        }

#if UNITY_2023_1_OR_NEWER
        public async Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds)
        {
            if (_dataLoaded) return true;
            LoadData();
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

namespace YG
{
    public partial class SavesYG
    {
        public SaveInfo saveInfo = new();
    }
}