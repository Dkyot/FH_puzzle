using Platforms.Ad;
using Platforms.Metrika;
using Platforms.Save;
using Platforms.User;
using UnityEngine;

namespace Platforms.Main
{
    public class YandexGamesFeaturesConfigurator : MonoBehaviour
    {
        [SerializeField] private string mainLeaderboardName;
        
        private void Awake()
        {
#if !UNITY_EDITOR
            RegisterFeatures();
#endif
        }

        private void RegisterFeatures()
        {
            var ad = new YandexGamesAdFeature();
            var save = new YandexGamesSaveFeature();
            var user = new YandexUserFeature(mainLeaderboardName);
            var metrika = new YandexGamesMetrikaFeature();
            
            ad.InitCallbacks();
            save.InitCallbacks();
            
            PlatformFeatures.Configure(ad, save, user, metrika);
        }
    }
}