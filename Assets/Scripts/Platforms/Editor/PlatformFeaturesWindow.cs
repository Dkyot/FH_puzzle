using System.Collections.Generic;
using Platforms.Features;
using Platforms.Main;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platforms.Editor
{
    public sealed class PlatformFeaturesWindow : PlatformWindowBase
    {
        private static SceneAsset _bootstrapScene;
        private static PlatformFeaturesSoBase _featuresSo;

        private const string bootstrapSceneKeyConst = "BootstrapScene";
        private const string featuresSoKeyConst = "CurrentFeatures";

        public PlatformFeaturesWindow() : base("FeaturesEnabled", "Enable features")
        {
        }

        protected override void DrawGUI()
        {
            EditorGUILayout.LabelField("Bootstrap scene");
            _bootstrapScene = (SceneAsset)EditorGUILayout.ObjectField(_bootstrapScene, typeof(SceneAsset), false);
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Platform features");
            _featuresSo = (PlatformFeaturesSoBase)EditorGUILayout.ObjectField(_featuresSo, 
                typeof(PlatformFeaturesSoBase), false);
        }

        protected override void PreprocessBuild(BuildReport report)
        {
            if (_bootstrapScene == null)
            {
                throw new BuildFailedException("BootstrapScene not set");
            }
            
            if (_featuresSo == null)
            {
                throw new BuildFailedException("Platform features not set");
            }

            if (_featuresSo.PlatformTargets.IndexOf(EditorUserBuildSettings.activeBuildTarget) == -1)
            {
                throw new BuildFailedException("Incorrect BuildTarget");
            }

            string currentOpenScenePath = SceneManager.GetActiveScene().path;
            var scene = EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(_bootstrapScene));
            if (!scene.IsValid())
            {
                throw new BuildFailedException("BootstrapScene is not valid");
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
                        Object.DestroyImmediate(configurators[i]);
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
                    var obj = Object.Instantiate(new GameObject("PlatformConfigure"));
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
        
        protected override void LoadRequiredData()
        {
            _bootstrapScene =
                AssetDatabase.LoadAssetAtPath<SceneAsset>(
                    AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(bootstrapSceneKeyConst)));
            _featuresSo =
                AssetDatabase.LoadAssetAtPath<PlatformFeaturesSoBase>(
                    AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(featuresSoKeyConst)));
        }

        protected override void SaveRequiredData()
        {
            EditorPrefs.SetString(bootstrapSceneKeyConst,
                AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_bootstrapScene)));
            EditorPrefs.SetString(featuresSoKeyConst,
                AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_featuresSo)));
        }
    }
}