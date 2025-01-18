using Platforms.Ad;
using Platforms.Metrika;
using Platforms.Save;
using Platforms.User;
using UnityEngine;
using YG;

namespace Platforms.Main
{
    public class YandexGamesFeaturesConfigurator : MonoBehaviour
    {
        [SerializeField] private string mainLeaderboardName;
        
        private void Awake()
        {
#if !UNITY_EDITOR
            RegisterFeaturesYandex();
#endif
        }

        private void RegisterFeaturesYandex()
        {
            var ad = new YandexGamesAdFeature();
            var save = new YandexGamesSaveFeature();
            var user = new YandexUserFeature(mainLeaderboardName);
            var metrika = new YandexGamesMetrikaFeature();
            
            ad.InitCallbacks();
            
            PlatformFeatures.Configure(ad, save, user, metrika);
        }
    }
}