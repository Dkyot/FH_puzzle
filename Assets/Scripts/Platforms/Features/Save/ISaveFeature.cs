using System;
using UnityEngine;

namespace Platforms.Save
{
    public interface ISaveFeature
    {
        SaveInfo SaveInfo { get; }
        
        event Action DataLoadedEvent;
        
        void LoadData();
        void SaveData();
        
#if UNITY_2023
        Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds);
#endif
    }
}