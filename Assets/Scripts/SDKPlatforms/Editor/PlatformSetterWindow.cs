using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SDKPlatforms.Features;
using SDKPlatforms.Main;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDKPlatforms.Editor
{
    public class PlatformSetterWindow : EditorWindow, IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        private static SceneAsset _bootstrapScene;
        private static int _currentFeatureIndex;
        private static string[] _featuresGuids;
        private static string[] _featuresNames;
        private static bool _saveDataLoaded;
        private static bool _enabled;

        private const string bootstrapSceneKeyConst = "PlatformFeatures_BootstrapScene";
        private const string currentFeatureKeyConst = "PlatformFeatures_CurrentFeature";
        private const string foundFeaturesKeyConst = "PlatformFeatures_FoundFeatures";
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
            var featureSo =
                AssetDatabase.LoadAssetAtPath<FeaturesSoBase>(
                    AssetDatabase.GUIDToAssetPath(_featuresGuids[_currentFeatureIndex]));
            if (featureSo == null)
            {
                throw new BuildFailedException("Platform feature not found");
            }

            if (featureSo.PlatformTargets.IndexOf(EditorUserBuildSettings.activeBuildTarget) == -1)
            {
                throw new BuildFailedException("Incorrect BuildTarget");
            }

            //Settings check
            if (featureSo.PlatformSettings != null)
            {
                featureSo.PlatformSettings.SetSettings();
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

            configurator.featuresSo = featureSo;

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
                EditorGUILayout.LabelField("Platform configure");
                _currentFeatureIndex = EditorGUILayout.Popup(_currentFeatureIndex, _featuresNames);
                EditorGUILayout.Space(5);

                if (GUILayout.Button("Search configures in project"))
                {
                    string currentConfigure = CheckCurrentConfigure() ? _featuresGuids[_currentFeatureIndex] : "";
                    _featuresGuids = AssetDatabase.FindAssets("t:" + nameof(FeaturesSoBase)).ToArray();
                    _currentFeatureIndex = Array.IndexOf(_featuresGuids, currentConfigure);
                    for (int i = 0; i < _featuresGuids.Length; i++)
                    {
                        _featuresNames[i] = AssetDatabase.LoadAssetAtPath<FeaturesSoBase>(_featuresGuids[i]).name;
                    }
                }
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

            _bootstrapScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorPrefs.GetString(bootstrapSceneKeyConst));

            var dictionary =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    EditorPrefs.GetString(foundFeaturesKeyConst)) ?? new Dictionary<string, string>();
            int featureCount = dictionary.Count;
            _featuresGuids = new string[featureCount];
            _featuresNames = new string[featureCount];
            int i = 0;
            foreach (var pair in dictionary)
            {
                _featuresGuids[i] = pair.Key;
                _featuresNames[i] = pair.Value;
                i++;
            }

            _currentFeatureIndex = Array.IndexOf(_featuresGuids, EditorPrefs.GetString(currentFeatureKeyConst));
            _enabled = EditorPrefs.GetBool(enabledKeyConst);
            _saveDataLoaded = true;
        }

        private static void SaveSettings()
        {
            EditorPrefs.SetString(bootstrapSceneKeyConst, AssetDatabase.GetAssetPath(_bootstrapScene));
            var dictionary = new Dictionary<string, string>();
            for (int index = 0; index < _featuresGuids.Length; index++)
            {
                dictionary.Add(_featuresGuids[index], _featuresNames[index]);
            }

            EditorPrefs.SetString(foundFeaturesKeyConst, JsonConvert.SerializeObject(dictionary));
            if (CheckCurrentConfigure())
            {
                EditorPrefs.SetString(currentFeatureKeyConst, _featuresGuids[_currentFeatureIndex]);
            }

            EditorPrefs.SetBool(enabledKeyConst, _enabled);
        }

        private static bool CheckCurrentConfigure()
        {
            return _currentFeatureIndex >= 0 && _currentFeatureIndex < _featuresGuids.Length;
        }

        [MenuItem("Tools/Platform Platform Setter")]
        private static void GetWindow()
        {
            var window = GetWindow<PlatformSetterWindow>();
            window.titleContent = new GUIContent("Platform Setter");
        }
    }
}