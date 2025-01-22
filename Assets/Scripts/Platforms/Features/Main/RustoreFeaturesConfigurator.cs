using System;
using Platforms.Ad;
using Platforms.Save;
using UnityEngine;

namespace Platforms.Main
{
    public class RustoreFeaturesConfigurator : MonoBehaviour
    {
        [SerializeField] private string saveKey;

        [SerializeField, Space, Header("Ad")] private string rewardId;
        [SerializeField] private string fullscreenId;
        [SerializeField] private int fullscreenTimeoutSeconds = 150;

        private void Awake()
        {
#if !UNITY_EDITOR
            Application.targetFrameRate = 120;
            RegisterFeaturesRustore();
#endif
        }

#if !UNITY_EDITOR
        private void RegisterFeaturesRustore()
        {
            var ad = new YandexMobileAdFeature(rewardId, fullscreenId, TimeSpan.FromSeconds(fullscreenTimeoutSeconds));
            var save = new PlayerPrefSaveFeature(saveKey);
            PlatformFeatures.Configure(ad, save);
        }

#endif
    }
}