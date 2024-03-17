using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Platform.Android;
using UnityEngine.Localization.Settings;

namespace Platforms.Settings.Editor
{
    [CreateAssetMenu(fileName = "_AndroidSettings", menuName = "Platform Settings/Android")]
    public class AndroidSettingsSo : PlatformSettingsSoBase
    {
        [SerializeField] private int bundleCodeVersion;
        [SerializeField] private Texture2D gameIcon;
        [SerializeField] private Texture2D splashIcon;
        [SerializeField] private LocalizedString gameLocalizedName;

        protected override void SetSpecificSettings()
        {
            PlayerSettings.Android.bundleVersionCode = bundleCodeVersion;
            bundleCodeVersion += 1;
            PlayerSettings.SetIcons(NamedBuildTarget.Unknown, new[] { gameIcon }, IconKind.Any);
            UpdateAndroidStaticSplashImage();
            if (gameLocalizedName != null)
            {
                var currentAppInfo = LocalizationSettings.Metadata.GetMetadata<AppInfo>() ?? new AppInfo();
                currentAppInfo.DisplayName = gameLocalizedName;
                LocalizationSettings.Metadata.AddMetadata(currentAppInfo);
            }
        }

        private void UpdateAndroidStaticSplashImage()
        {
            const string projectSettings = "ProjectSettings/ProjectSettings.asset";
            var settingsManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(projectSettings)[0]);
            var splashScreen = settingsManager.FindProperty("androidSplashScreen");
            splashScreen.objectReferenceValue = splashIcon;
            settingsManager.ApplyModifiedProperties();
            settingsManager.Update();
        }
    }
}