#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Platforms.Settings.Editor
{
    public abstract class PlatformSettingsSoBase : ScriptableObject
    {
        [SerializeField] private string productName;
        [SerializeField] private string version = "1.0";
        [SerializeField] private Color splashColor;
        [SerializeField] private Sprite splashBackground;
        
        public void SetSettings()
        {
            EditorUtility.SetDirty(this);
            PlayerSettings.productName = productName;
            PlayerSettings.bundleVersion = version;
            PlayerSettings.SplashScreen.backgroundColor = splashColor;
            PlayerSettings.SplashScreen.background = splashBackground;
            //you can add general settings here
            SetSpecificSettings();
            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("File/Save Project");
            AssetDatabase.Refresh();
            AssetDatabase.RefreshSettings();
        }
        
        protected abstract void SetSpecificSettings();
    }
}
#endif