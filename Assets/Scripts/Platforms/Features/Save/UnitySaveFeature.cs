using System;
using UnityEngine;

namespace Platforms.Save
{
    public class UnitySaveFeature : ISaveFeature
    {
        public SaveInfo SaveInfo { get; private set; } = new();
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