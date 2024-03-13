using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SDKPlatforms.Settings
{
    public abstract class PlatformSettingsSoBase : ScriptableObject
    {
        [SerializeField] private string productName;
        [SerializeField] private Color splashColor;
        [SerializeField] private Sprite splashBackground;
        
        public void SetSettings()
        {
            EditorUtility.SetDirty(this);
            PlayerSettings.productName = productName;
            PlayerSettings.bundleVersion = GenerateVersion();
            PlayerSettings.SplashScreen.backgroundColor = splashColor;
            PlayerSettings.SplashScreen.background = splashBackground;
            //you can add general settings here
            SetSpecificSettings();
            AssetDatabase.SaveAssets();
        }
        
        protected abstract void SetSpecificSettings();
        protected abstract string GenerateVersion();
    }
}