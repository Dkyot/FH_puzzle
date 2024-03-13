using System;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SDKPlatforms.Editor
{
    public class PlatformProjectSettingsWindow : EditorWindow, IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        private static int _currentConfigureIndex;
        private static string[] _configuresPath;
        private static bool _saveDataLoaded;
        private static bool _enabled;

        private const string currentConfigureKeyConst = "PlatformProjectSettings_CurrentConfigure";
        private const string foundConfiguresKeyConst = "PlatformProjectSettings_FoundConfigures";
        private const string enabledKeyConst = "PlatformProjectSettings_Enabled";

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!_saveDataLoaded)
            {
                Awake();
            }
        }

        public void OnGUI()
        {
            if (!_saveDataLoaded)
            {
                Awake();
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                SaveSettings();
            }
        }

        private void Awake()
        {
            _configuresPath = JsonConvert.DeserializeObject<string[]>(EditorPrefs.GetString(foundConfiguresKeyConst)) ??
                              Array.Empty<string>();
            _currentConfigureIndex = Array.IndexOf(_configuresPath, EditorPrefs.GetString(currentConfigureKeyConst));
            _enabled = EditorPrefs.GetBool(enabledKeyConst);
            _saveDataLoaded = true;
        }
        
        private static void SaveSettings()
        {
            EditorPrefs.SetString(foundConfiguresKeyConst, JsonConvert.SerializeObject(_configuresPath));
            if (CheckCurrentConfigure())
            {
                EditorPrefs.SetString(currentConfigureKeyConst, _configuresPath[_currentConfigureIndex]);
            }

            EditorPrefs.SetBool(enabledKeyConst, _enabled);
        }

        private static bool CheckCurrentConfigure()
        {
            return _currentConfigureIndex >= 0 && _currentConfigureIndex < _configuresPath.Length;
        }

        [MenuItem("Tools/ Platform Features Settings")]
        private static void GetWindow()
        {
            var window = GetWindow<PlatformProjectSettingsWindow>();
            window.titleContent = new GUIContent("Platform project settings");
        }
    }
}