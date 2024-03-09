using System;
using UnityEngine;

namespace PlatformsSdk.SaveFeatures
{
    public class UnitySaveFeature : ISaveFeature
    {
        public SaveInfo SaveInfo { get; private set; }
        public event Action DataLoadedEvent;

        public void SetDebugInfo(SaveInfo saveInfo)
        {
            SaveInfo = saveInfo;
            LoadData();
        }
        
        public void LoadData()
        {
            DataLoadedEvent?.Invoke();
        }

        public void SaveData()
        {
        }

        public async Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds)
        {
            LoadData();
            await Awaitable.NextFrameAsync();
            return true;
        }
    }
}