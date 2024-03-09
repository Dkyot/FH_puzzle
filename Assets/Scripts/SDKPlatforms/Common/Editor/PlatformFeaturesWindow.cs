using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Newtonsoft.Json;
using PlatformsSdk.FeaturesConfigures;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Utils.Editor;
using Object = UnityEngine.Object;

namespace PlatformsSdk.Common.Editor
{
    public class PlatformFeaturesWindow : EditorWindow, IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        private static SceneAsset _bootstrapScene;
        private static int _currentConfigureIndex;
        private static string[] _configuresPath;
        private static bool _saveDataLoaded;
        private static bool _enabled;

        private const string bootstrapSceneKeyConst = "PlatformFeatures_BootstrapScene";
        private const string currentConfigureKeyConst = "PlatformFeatures_CurrentConfigure";
        private const string foundConfiguresKeyConst = "PlatformFeatures_FoundConfigures";
        private const string enabledKeyConst = "PlatformFeatures_Enabled";

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!_saveDataLoaded)
            {
                Awake();
            }
            
            if(!_enabled) return;

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
            
            var prefab = AssetDatabase.LoadAssetAtPath<Object>(_configuresPath[_currentConfigureIndex]);
            if (prefab == null)
            {
                throw new BuildFailedException("Platform configure prefab not found");
            }

            EditorSceneManager.MarkSceneDirty(scene);
            //Delete previously installed configurations
            var prevConfigure = scene.GetRootGameObjects()
                .Where(x => x.GetComponentInChildren<FeaturesConfigureBase>() != null);
            foreach (var configure in prevConfigure)
            {
                DestroyImmediate(configure.gameObject);
            }
            
            var obj = Instantiate(prefab);
            obj.name = "PlatformConfigure";
            EditorSceneManager.SaveScene(scene);
            EditorSceneManager.OpenScene(currentOpenScenePath);
        }

        public void OnGUI()
        {
            if (!_saveDataLoaded)
            {
                Awake();
            }

            EditorGUI.BeginChangeCheck();
            _enabled = EditorGUILayout.Toggle(new GUIContent("Enable"), _enabled);
            if (_enabled)
            {
                EditorGUILayout.LabelField("Bootstrap scene");
                _bootstrapScene = (SceneAsset)EditorGUILayout.ObjectField(_bootstrapScene, typeof(SceneAsset), false);
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("Platform configure");
                _currentConfigureIndex = EditorGUILayout.Popup(_currentConfigureIndex, _configuresPath);
                EditorGUILayout.Space(5);

                if (GUILayout.Button("Search configures in project"))
                {
                    string currentConfigure = _configuresPath[_currentConfigureIndex];
                    _configuresPath = AssetHelper.FindPrefabsPathWithType<FeaturesConfigureBase>().ToArray();
                    _currentConfigureIndex = Array.IndexOf(_configuresPath, currentConfigure);
                }
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                SaveSettings();
            }
        }

        private void Awake()
        {
            _bootstrapScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorPrefs.GetString(bootstrapSceneKeyConst));
            _configuresPath = JsonConvert.DeserializeObject<string[]>(EditorPrefs.GetString(foundConfiguresKeyConst)) ??
                              Array.Empty<string>();
            _currentConfigureIndex = Array.IndexOf(_configuresPath, EditorPrefs.GetString(currentConfigureKeyConst));
            _enabled = EditorPrefs.GetBool(enabledKeyConst);
            _saveDataLoaded = true;
        }

        private static void SaveSettings()
        {
            EditorPrefs.SetString(bootstrapSceneKeyConst, AssetDatabase.GetAssetPath(_bootstrapScene));
            EditorPrefs.SetString(foundConfiguresKeyConst, JsonConvert.SerializeObject(_configuresPath));
            if (_currentConfigureIndex >= 0 && _currentConfigureIndex < _configuresPath.Length)
            {
                EditorPrefs.SetString(currentConfigureKeyConst, _configuresPath[_currentConfigureIndex]);
            }

            EditorPrefs.SetBool(enabledKeyConst, _enabled);
        }

        [MenuItem("Tools/ Platform Features Settings")]
        private static void GetWindow()
        {
            var window = GetWindow<PlatformFeaturesWindow>();
            window.titleContent = new GUIContent("Platform Features");
        }
    }
}