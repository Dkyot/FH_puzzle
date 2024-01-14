using System;
using UnityEngine;
using YG;

namespace PlatformFeatures.SaveFeatures
{
    public class YandexGamesSaveFeatures : SaveFeaturesBase
    {
        [SerializeField] private YandexGame yandexGamePrefab;
        
        public override event Action DataLoadedEvent;
        
        private void OnEnable()
        {
            YandexGame.GetDataEvent += DataLoadedEvent;
        }

        private void OnDisable()
        {
            YandexGame.GetDataEvent -= DataLoadedEvent;
        }

        protected override void Init()
        {
            if (yandexGamePrefab == null)
            {
                Debug.LogError("No Yandex plugin prefab");
            }
        }

        public override void LoadData()
        {
            if (YandexGame.SDKEnabled)
            {
                DataLoadedEvent?.Invoke();
            }
        }

        public override void SaveData()
        {
            YandexGame.SaveProgress();
        }
    }
}