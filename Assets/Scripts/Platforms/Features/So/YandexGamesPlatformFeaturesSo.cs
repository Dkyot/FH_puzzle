using Platforms.Ad;
using Platforms.Main;
using Platforms.Metrika;
using Platforms.Save;
using Platforms.User;
using UnityEngine;
using YG;

namespace Platforms.Features
{
    [CreateAssetMenu(fileName = "_YandexGamesFeatures", menuName = "Platform Features/Yandex Games")]
    public class YandexGamesFeaturesSo : PlatformFeaturesSoBase
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
            PlatformFeatures.Configure(adFeature, saveFeature, userFeature, metrikaFeature);
        }
    }
}