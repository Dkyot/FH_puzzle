using System.Collections.Generic;
using System.Linq;
using Platforms.Scenes.Editor;
using Platforms.Settings.Editor;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Platforms.Editor
{
    public class PlatformScenesWindow : PlatformWindowBase
    {
        private static PlatformScenesSo _scenesSo;

        private const string scenesSoKeyConst = "CurrentScenes";

        public PlatformScenesWindow() : base("ScenesEnabled", "Enabled scenes")
        {
        }

        protected override void DrawGUI()
        {
            EditorGUILayout.LabelField("Platform scenes");
            _scenesSo =
                (PlatformScenesSo)EditorGUILayout.ObjectField(_scenesSo, typeof(PlatformScenesSo),
                    false);
        }

        protected override void PreprocessBuild(BuildReport report)
        {
            if (_scenesSo == null)
            {
                throw new BuildFailedException("Platform scenes not set");
            }

            // Set the Build Settings window Scene list
            EditorBuildSettings.scenes = (from sceneAsset in _scenesSo.Scenes
                select AssetDatabase.GetAssetPath(sceneAsset)
                into scenePath
                where !string.IsNullOrEmpty(scenePath)
                select new EditorBuildSettingsScene(scenePath, true)).ToArray();
        }

        protected override void LoadRequiredData()
        {
            _scenesSo = AssetDatabase.LoadAssetAtPath<PlatformScenesSo>(
                AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(scenesSoKeyConst)));
        }

        protected override void SaveRequiredData()
        {
            EditorPrefs.SetString(scenesSoKeyConst,
                AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_scenesSo)));
        }
    }
}