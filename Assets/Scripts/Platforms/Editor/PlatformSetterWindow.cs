using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Platforms.Features;
using Platforms.Main;
using Platforms.Settings.Editor;
using Platforms.Settings;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platforms.Editor
{
    public class PlatformSetterWindow : EditorWindow, IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        private static SceneAsset _bootstrapScene;
        private static PlatformFeaturesSoBase _featuresSo;
        private static PlatformSettingsSoBase _settingsSo;
        private static bool _saveDataLoaded;
        private static bool _enabled;

        private const string bootstrapSceneKeyConst = "PlatformFeatures_BootstrapScene";
        private const string featuresSoKeyConst = "PlatformFeatures_CurrentFeatures";
        private const string settingsSoKeyConst = "PlatformFeatures_CurrentPlatformSettings";
        private const string enabledKeyConst = "PlatformFeatures_Enabled";

        public void OnPreprocessBuild(BuildReport report)
        {
            LoadSettings();

            if (!_enabled) return;

            //Scene check
            if (_bootstrapScene == null)
            {
                throw new BuildFailedException("BootstrapScene not set");
            }

            string currentOpenScenePath = SceneManager.GetActiveScene().path;
            var scene = EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(_bootstrapScene));
            if (!scene.IsValid())
            {
                throw new BuildFailedException("BootstrapScene is not valid");
            }

            if (scene.buildIndex <= 0)
            {
                throw new BuildFailedException("BootstrapScene is not set in build scene");
            }

            //Features check
            if (_featuresSo == null)
            {
                throw new BuildFailedException("Platform features not set");
            }

            if (_featuresSo.PlatformTargets.IndexOf(EditorUserBuildSettings.activeBuildTarget) == -1)
            {
                throw new BuildFailedException("Incorrect BuildTarget");
            }

            //Settings check
            if (_settingsSo != null)
            {
                _settingsSo.SetSettings();
            }

            //Prepare bootstrap scene
            EditorSceneManager.MarkSceneDirty(scene);
            var configurators = new List<FeaturesConfigurator>();

            foreach (var rootObj in scene.GetRootGameObjects())
            {
                var comps = rootObj.GetComponentsInChildren<FeaturesConfigurator>();
                if (comps.Length != 0)
                {
                    configurators.AddRange(comps);
                }
            }

            FeaturesConfigurator configurator = null;
            switch (configurators.Count)
            {
                case > 1:
                {
                    for (int i = 1; i < configurators.Count; i++)
                    {
                        DestroyImmediate(configurators[i]);
                    }

                    configurator = configurators[0];
                    break;
                }
                case 1:
                {
                    configurator = configurators[0];
                    break;
                }
                case 0:
                {
                    var obj = Instantiate(new GameObject("PlatformConfigure"));
                    configurator = obj.AddComponent<FeaturesConfigurator>();
                    break;
                }
            }

            if (configurator == null)
            {
                throw new BuildFailedException("FeaturesConfigurator setup error");
            }

            configurator.featuresSo = _featuresSo;

            EditorSceneManager.SaveScene(scene);
            EditorSceneManager.OpenScene(currentOpenScenePath);
        }

        public void OnGUI()
        {
            LoadSettings();

            EditorGUI.BeginChangeCheck();
            _enabled = EditorGUILayout.Toggle(new GUIContent("Enable"), _enabled);
            if (_enabled)
            {
                EditorGUILayout.LabelField("Bootstrap scene");
                _bootstrapScene = (SceneAsset)EditorGUILayout.ObjectField(_bootstrapScene, typeof(SceneAsset), false);
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("Platform features");
                _featuresSo =
                    (PlatformFeaturesSoBase)EditorGUILayout.ObjectField(_featuresSo, typeof(PlatformFeaturesSoBase),
                        false);
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("Platform settings");
                _settingsSo =
                    (PlatformSettingsSoBase)EditorGUILayout.ObjectField(_settingsSo, typeof(PlatformSettingsSoBase),
                        false);
            }

            if (EditorGUI.EndChangeCheck())
            {
                SaveSettings();
            }
        }

        private void Awake()
        {
            LoadSettings();
        }

        private static void LoadSettings()
        {
            if (_saveDataLoaded) return;
            _bootstrapScene =
                AssetDatabase.LoadAssetAtPath<SceneAsset>(
                    AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(bootstrapSceneKeyConst)));
            _featuresSo =
                AssetDatabase.LoadAssetAtPath<PlatformFeaturesSoBase>(
                    AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(featuresSoKeyConst)));
            _settingsSo = AssetDatabase.LoadAssetAtPath<PlatformSettingsSoBase>(
                AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(settingsSoKeyConst)));
            _enabled = EditorPrefs.GetBool(enabledKeyConst);
            _saveDataLoaded = true;
        }

        private static void SaveSettings()
        {
            EditorPrefs.SetString(bootstrapSceneKeyConst,
                AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_bootstrapScene)));
            EditorPrefs.SetString(featuresSoKeyConst,
                AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_featuresSo)));
            EditorPrefs.SetString(settingsSoKeyConst,
                AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_settingsSo)));
            EditorPrefs.SetBool(enabledKeyConst, _enabled);
        }

        [MenuItem("Tools/Platform Setter")]
        private static void GetWindow()
        {
            var window = GetWindow<PlatformSetterWindow>();
            window.titleContent = new GUIContent("Platform Setter");
        }
    }
}