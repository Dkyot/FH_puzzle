using System;
using UnityEngine;

namespace PlatformFeatures.SaveFeatures
{
    public abstract class SaveFeaturesBase : MonoBehaviour
    {
        public static SaveFeaturesBase Instance { get; private set; }
        public SaveInfo SaveInfo { get; protected set; } = new SaveInfo();
        
        public abstract event Action DataLoadedEvent;

        protected virtual void Awake()
        {
            Init();
            Instance = this;
        }

        protected abstract void Init();

        public abstract void LoadData();
        public abstract void SaveData();
    }
}