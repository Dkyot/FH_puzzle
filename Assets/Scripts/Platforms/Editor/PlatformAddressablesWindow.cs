using Platforms.Addressables;
using Platforms.Settings.Editor;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Platforms.Editor
{
    public class PlatformAddressablesWindow : PlatformWindowBase
    {
        private static PlatformAddressablesSo _addressablesSo;

        private const string addressablesSoKeyConst = "CurrentPlatformAddressables";

        public PlatformAddressablesWindow() : base("AddressablesEnabled", "Enable addressables")
        {
        }

        protected override void DrawGUI()
        {
            EditorGUILayout.LabelField("Platform addressables");
            _addressablesSo =
                (PlatformAddressablesSo)EditorGUILayout.ObjectField(_addressablesSo, typeof(PlatformAddressablesSo),
                    false);
        }

        protected override void PreprocessBuild(BuildReport report)
        {
            if (_addressablesSo == null)
            {
                throw new BuildFailedException("Platform addressables not set");
            }

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            foreach (var group in settings.groups)
            {
                if (group.ReadOnly || group.Default) continue;
                var groupSchema = group.GetSchema<BundledAssetGroupSchema>();
                groupSchema.IncludeInBuild = _addressablesSo.Addressables.Contains(group);
                EditorUtility.SetDirty(group);
            }
        }

        protected override void LoadRequiredData()
        {
            _addressablesSo = AssetDatabase.LoadAssetAtPath<PlatformAddressablesSo>(
                AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString(addressablesSoKeyConst)));
        }

        protected override void SaveRequiredData()
        {
            EditorPrefs.SetString(addressablesSoKeyConst,
                AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_addressablesSo)));
        }
    }
}