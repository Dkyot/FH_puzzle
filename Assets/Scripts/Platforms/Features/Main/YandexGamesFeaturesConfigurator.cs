// using Platforms.Ad;
// using Platforms.Metrika;
// using Platforms.Save;
// using Platforms.User;
// using UnityEngine;
// #if !UNITY_EDITOR
// using YG;
// #endif
//
// namespace Platforms.Main
// {
//     public class YandexGamesFeaturesConfigurator : MonoBehaviour
//     {
//         [SerializeField] private string mainLeaderboardName;
//         
//         private void Awake()
//         {
// #if !UNITY_EDITOR
//             RegisterFeaturesYandex();
// #endif
//         }
//
// #if !UNITY_EDITOR
//         private void RegisterFeaturesYandex()
//         {
//             var ad = new YandexGamesAdFeature();
//             var save = new YandexGamesSaveFeature();
//             var user = new YandexUserFeature(mainLeaderboardName);
//             var metrika = new YandexGamesMetrikaFeature();
//             
//             ad.InitCallbacks();
//             
//             PlatformFeatures.Configure(ad, save, user, metrika);
//         }
// #endif
//     }
// }