using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Platforms.Editor
{
    public sealed class PlatformSetterWindow : EditorWindow, IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        private static bool _saveDataLoaded;

        private static readonly List<PlatformWindowBase> WindowParts = new()
        {
            new PlatformFeaturesWindow(),
            new PlatformSettingsWindow(),
            new PlatformScenesWindow(),
            new PlatformAddressablesWindow()
        };
        
        public void OnGUI()
        {
            foreach (var part in WindowParts)
            {
                part.OnGUI();
                EditorGUILayout.Space(10);
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            foreach (var part in WindowParts)
            {
                part.OnPreprocessBuild(report);
            }
        }
        
        [MenuItem("Tools/Platform Setter")]
        private static void GetWindow()
        {
            var window = GetWindow<PlatformSetterWindow>();
            window.titleContent = new GUIContent("Platform Setter");
        }
    }
}