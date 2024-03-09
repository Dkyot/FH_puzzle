using PlatformsSdk.AdFeatures;
using PlatformsSdk.Main;
using PlatformsSdk.MetrikaFeatures;
using PlatformsSdk.SaveFeatures;
using PlatformsSdk.UserFeatures;
using UnityEngine;
using YG;

namespace PlatformsSdk.FeaturesConfigures
{
    public class YandexGameFeaturesConfigure : FeaturesConfigureBase
    {
        [SerializeField] private GameObject yandexPrefab;
        [SerializeField] private string mainLeaderboardName;

        protected override void RegisterFeatures()
        {
            var yandex = Instantiate(yandexPrefab).GetComponent<YandexGame>();
            var adFeature = new YandexAdFeature(yandex.infoYG);
            var saveFeature = new YandexSaveFeature();
            var userFeature = new YandexUserFeature(yandex.infoYG, mainLeaderboardName);
            var metrikaFeature = new YandexMetrikaFeature(yandex.infoYG);
            adFeature.InitCallbacks();
            saveFeature.InitCallbacks();
            PlatformFeatures.Configure(adFeature, saveFeature, userFeature, metrikaFeature);
        }
    }
}