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

        private bool _dataLoaded;
        private bool _callbackInit;

        public YandexGamesSaveFeature()
        {
            if (YG2.isSDKEnabled)
            {
                LoadData();
            }
        }
        
        public void InitCallbacks()
        {
            if(_callbackInit) return;

            YG2.onGetSDKData += DataLoadedEvent;
            DataLoadedEvent += LoadData;
            _callbackInit = true;
        }
        
        ~YandexGamesSaveFeature()
        {
            YG2.onGetSDKData -= DataLoadedEvent;
            DataLoadedEvent -= LoadData;
        }

        public void LoadData()
        {
            if (_dataLoaded) return;
            YGInsides.LoadProgress();
            if (string.IsNullOrEmpty(SaveInfo.Language))
            {
                YG2.GetEnvirData();
                SaveInfo.Language = YG2.envir.language;
            }
            
            Debug.Log(JsonConvert.SerializeObject(SaveInfo));
            
            _dataLoaded = true;
            DataLoadedEvent?.Invoke();
        }

        public void SaveData()
        {
            SaveInfo.LastSaveTimeTicks = DateTime.UtcNow.Ticks;
            YG2.SaveProgress();
        }

#if UNITY_2023_1_OR_NEWER
        public async Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds)
        {
            if (YG2.isSDKEnabled)
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

namespace YG
{
    public partial class SavesYG
    {
        public SaveInfo saveInfo = new();
    }
}