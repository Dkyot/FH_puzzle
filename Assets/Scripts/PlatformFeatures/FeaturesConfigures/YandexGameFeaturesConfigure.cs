using PlatformFeatures.AdFeatures;
using UnityEngine;
using YG;

namespace PlatformFeatures.FeaturesConfigures
{
    public class YandexGameFeaturesConfigure : FeaturesConfigureBase
    {
        [SerializeField] private GameObject yandexPrefab;

        protected override void RegisterFeatures()
        {
            var yandex = Instantiate(yandexPrefab).GetComponent<YandexGame>();
            var adFeature = new YandexAdFeature(yandex.infoYG);
            adFeature.InitCallbacks();
            Common.PlatformFeatures.Configure(adFeature);
        }
    }
}