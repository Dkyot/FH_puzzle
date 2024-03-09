﻿using PlatformsSdk.AdFeatures;
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
            adFeature.InitCallbacks();
            saveFeature.InitCallbacks();
            Main.PlatformFeatures.Configure(adFeature, saveFeature, userFeature);
        }
    }
}