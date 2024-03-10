using System;
using PlatformsSdk.AdFeatures;
using PlatformsSdk.Main;
using PlatformsSdk.SaveFeatures;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlatformsSdk.FeaturesConfigures
{
    public class RustoreFeaturesConfigure : FeaturesConfigureBase
    {
        [SerializeField] private string saveKey;
        [SerializeField] private string rewardId;
        [SerializeField] private string fullscreenId;
        [SerializeField] private int fullscreenTimeoutSeconds;

        protected override void RegisterFeatures()
        {
            var adFeature = new YandexMobileAdFeature(rewardId, fullscreenId,
                TimeSpan.FromSeconds(fullscreenTimeoutSeconds));
            var saveFeature = new PlayerPrefSaveFeature(saveKey);

            PlatformFeatures.Configure(adFeature, saveFeature);
        }
    }
}