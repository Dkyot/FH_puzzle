using System;
using Newtonsoft.Json;
using UnityEngine;

namespace PlatformFeatures.SaveFeatures
{
    public class UnityEditorSaveFeatures : SaveFeaturesBase
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
    }
}