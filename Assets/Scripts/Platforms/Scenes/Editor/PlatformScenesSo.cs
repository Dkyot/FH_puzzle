using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Platforms.Scenes.Editor
{
    public class PlatformScenesSo : ScriptableObject
    {
        [field:SerializeField] public List<SceneAsset> Scenes { get; private set; }
    }
}