using PlatformsSdk.AdFeatures;
using PlatformsSdk.SaveFeatures;
using UnityEngine;
using YG;

namespace PlatformsSdk.FeaturesConfigures
{
    public class YandexGameFeaturesConfigure : FeaturesConfigureBase
    {
        [SerializeField] private GameObject yandexPrefab;

        protected override void RegisterFeatures()
        {
            var yandex = Instantiate(yandexPrefab).GetComponent<YandexGame>();
            var adFeature = new YandexAdFeature(yandex.infoYG);
            var saveFeature = new YandexSaveFeature();
            adFeature.InitCallbacks();
            saveFeature.InitCallbacks();
            Main.PlatformFeatures.Configure(adFeature, saveFeature);
        }
    }
}