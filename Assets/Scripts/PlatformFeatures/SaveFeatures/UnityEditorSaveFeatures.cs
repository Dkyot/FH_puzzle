using System;
using Newtonsoft.Json;
using UnityEngine;

namespace PlatformFeatures.SaveFeatures
{
    public class UnityEditorSaveFeatures : SaveFeatures
    {
        [SerializeField] private bool useDebugData;
        [SerializeField] private SaveInfo debugData;

        public override event Action DataLoadedEvent;

        protected override void Init()
        {
            DataLoadedEvent?.Invoke();
        }

        public override void LoadData()
        {
            if (useDebugData)
            {
                SaveInfo = debugData;
            }

            DataLoadedEvent?.Invoke();
        }

        public override void SaveData()
        {
        }

#if UNITY_2023
        public override async Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds)
        {
            LoadData();
            await Awaitable.NextFrameAsync();
            return true;
        }
#endif
    }
}