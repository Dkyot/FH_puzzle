using System;
using Platforms.Ad;
using Platforms.Save;
using Platforms.Main;
using UnityEngine;

namespace Platforms.Features
{
    [CreateAssetMenu(fileName = "_RustoreFeatures", menuName = "Platform/Features/Rustore")]
    public class RustoreFeaturesSo : PlatformFeaturesSoBase
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

            PlatformFeatures.Configure(adFeature, saveFeature);
        }
    }
}