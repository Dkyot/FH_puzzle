using System.Collections.Generic;
using SDKPlatforms.Settings;
using UnityEditor;
using UnityEngine;

namespace SDKPlatforms.Features
{
    public abstract class PlatformFeaturesSoBase : ScriptableObject
    {
#if UNITY_EDITOR
        [field:SerializeField] public List<BuildTarget> PlatformTargets { get; private set; }
        [field: SerializeField] public PlatformSettingsSoBase PlatformSettings { get; private set; }
#endif
        
        public abstract void RegisterFeatures();
    }
}