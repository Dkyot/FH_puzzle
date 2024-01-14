﻿using System;
using UnityEngine;

namespace PlatformFeatures.SaveFeatures
{
    public abstract class SaveFeatures : MonoBehaviour
    {
        public static SaveFeatures Instance { get; private set; }
        public SaveInfo SaveInfo { get; set; } = new SaveInfo();
        
        public abstract event Action DataLoadedEvent;

        protected virtual void Awake()
        {
            Init();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        protected abstract void Init();

        public abstract void LoadData();
        public abstract void SaveData();
        
#if UNITY_2023
        public abstract Awaitable<bool> LoadDataAwaitable(uint waitingTimeSeconds);
#endif
    }
}