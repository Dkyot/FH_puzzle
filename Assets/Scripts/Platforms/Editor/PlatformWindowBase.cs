using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Platforms.Editor
{
    public abstract class PlatformWindowBase
    {
        private bool _enabled;
        private bool _saveDataLoaded;

        private readonly string _enabledSaveKey;
        private readonly string _enabledText;

        protected PlatformWindowBase(string enabledSaveKey, string enabledText)
        {
            _enabledSaveKey = enabledSaveKey;
            _enabledText = enabledText;
        }
        
        public void OnGUI()
        {
            LoadSettings();
            EditorGUI.BeginChangeCheck();
            _enabled = EditorGUILayout.Toggle(new GUIContent(_enabledText), _enabled);
            if (_enabled)
            {
                DrawGUI();
            }
            if (EditorGUI.EndChangeCheck())
            {
                SaveSettings();
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!_enabled) return;
            LoadSettings();
            PreprocessBuild(report);
        }

        private void LoadSettings()
        {
            if (_saveDataLoaded) return;
            LoadRequiredData();
            _saveDataLoaded = true;
        }
        
        private void SaveSettings()
        {
            EditorPrefs.SetBool(_enabledSaveKey, _enabled);
            SaveRequiredData();
        }

        protected abstract void LoadRequiredData();
        protected abstract void SaveRequiredData();
        protected abstract void DrawGUI();
        protected abstract void PreprocessBuild(BuildReport report);
    }
}