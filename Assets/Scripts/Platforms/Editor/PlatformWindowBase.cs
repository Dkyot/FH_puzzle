﻿using UnityEditor;
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
            LoadSettings();
            if (!_enabled) return;
            PreprocessBuild(report);
            EditorApplication.ExecuteMenuItem("File/Save Project");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.RefreshSettings();
        }

        private void LoadSettings()
        {
            if (_saveDataLoaded) return;
            _enabled = EditorPrefs.GetBool(_enabledSaveKey);
            LoadRequiredData();
            _saveDataLoaded = true;
        }
        
        private void SaveSettings()
        {
            EditorPrefs.SetBool(_enabledSaveKey, _enabled);
            SaveRequiredData();
        }

        protected abstract void DrawGUI();
        protected abstract void PreprocessBuild(BuildReport report);
        protected abstract void LoadRequiredData();
        protected abstract void SaveRequiredData();
    }
}