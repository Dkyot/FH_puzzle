using Platforms.Settings.Editor;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Platforms.Editor
{
    public class PlatformSettingsWindow : PlatformWindowBase
    {
        private static PlatformSettingsSoBase _projectSettingsSo;
        
        private const string settingsSoKeyConst = "CurrentPlatformSettings";
        
        public PlatformSettingsWindow() : base("SettingsEnabled", "Enabled settings")
        {
        }

        protected override void LoadRequiredData()
        {
            _projectSettingsSo = AssetDatabase.LoadAssetAtPath<PlatformSettingsSoBase>(
                AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(settingsSoKeyConst)));
        }

        protected override void SaveRequiredData()
        {
            EditorPrefs.SetString(settingsSoKeyConst,
                AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_projectSettingsSo)));
        }

        protected override void DrawGUI()
        {
            EditorGUILayout.LabelField("Platform settings");
            _projectSettingsSo =
                (PlatformSettingsSoBase)EditorGUILayout.ObjectField(_projectSettingsSo, typeof(PlatformSettingsSoBase),
                    false);
        }

        protected override void PreprocessBuild(BuildReport report)
        {
            //Settings check
            if (_projectSettingsSo == null)
            {
                throw new BuildFailedException("Platform settings not set");
            }
            _projectSettingsSo.SetSettings();
        }
    }
}