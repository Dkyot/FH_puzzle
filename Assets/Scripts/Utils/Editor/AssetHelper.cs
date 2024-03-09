using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class AssetHelper
    {
        public static List<string> FindPrefabsPathWithType<T>() where T : MonoBehaviour
        {
            string[] guids = AssetDatabase.FindAssets("t:prefab");
            var assetsPath = guids.Select(AssetDatabase.GUIDToAssetPath);
            var list = assetsPath.Where(path => AssetDatabase.LoadAssetAtPath<T>(path) != null).ToList();
            return list;
        }
    }
}