using System;
using SDKPlatforms.Ad;
using SDKPlatforms.Main;
using SDKPlatforms.Save;
using UnityEngine;

namespace SDKPlatforms.Features
{
    [CreateAssetMenu(fileName = "_RustoreFeatures", menuName = "Platform Features/Rustore")]
    public class RustoreFeaturesSo : FeaturesSoBase
    {
        [SerializeField] private string saveKey;

        [SerializeField, Space, Header("Ad")] private string rewardId;
        [SerializeField] private string fullscreenId;
        [SerializeField] private int fullscreenTimeoutSeconds = 60;

        public override void RegisterFeatures()
        {
            var adFeature = new YandexMobileAdFeature(rewardId, fullscreenId,
                TimeSpan.FromSeconds(fullscreenTimeoutSeconds));
            var saveFeature = new PlayerPrefSaveFeature(saveKey);

            Main.PlatformFeatures.Configure(adFeature, saveFeature);
        }
    }
}