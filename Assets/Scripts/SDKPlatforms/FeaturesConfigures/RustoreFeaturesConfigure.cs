using PlatformsSdk.AdFeatures;
using PlatformsSdk.Main;
using PlatformsSdk.SaveFeatures;
using UnityEngine;

namespace PlatformsSdk.FeaturesConfigures
{
    public class RustoreFeaturesConfigure: FeaturesConfigureBase
    {
        [SerializeField] private string saveKey;
        [SerializeField] private string adUnitId;
        
        protected override void RegisterFeatures()
        {
            var adFeature = new YandexMobileAdFeature(adUnitId);
            var saveFeature = new PlayerPrefSaveFeature(saveKey);
            
            PlatformFeatures.Configure(adFeature, saveFeature);
        }
    }
}