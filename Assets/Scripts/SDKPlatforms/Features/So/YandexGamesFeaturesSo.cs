using SDKPlatforms.Ad;
using SDKPlatforms.Main;
using SDKPlatforms.Metrika;
using SDKPlatforms.Save;
using SDKPlatforms.User;
using UnityEngine;
using YG;

namespace SDKPlatforms.Features
{
    public class YandexGamesFeaturesSo : FeaturesSoBase
    {
        [SerializeField] private GameObject yandexPrefab;
        [SerializeField] private string mainLeaderboardName;

        public override void RegisterFeatures()
        {
            var yandex = Instantiate(yandexPrefab).GetComponent<YandexGame>();
            var adFeature = new YandexGamesAdFeature(yandex.infoYG);
            var saveFeature = new YandexGamesSaveFeature();
            var userFeature = new YandexUserFeature(yandex.infoYG, mainLeaderboardName);
            var metrikaFeature = new YandexGamesMetrikaFeature(yandex.infoYG);
            adFeature.InitCallbacks();
            saveFeature.InitCallbacks();
            Main.PlatformFeatures.Configure(adFeature, saveFeature, userFeature, metrikaFeature);
        }
    }
}