using SDKPlatforms.Settings;
using UnityEngine;

namespace SDKPlatforms.Features
{
    public abstract class FeaturesSoBase : ScriptableObject
    {
#if UNITY_EDITOR
        [field: SerializeField] public PlatformSettingsSoBase PlatformSettings { get; private set; }
#endif
        
        public abstract void RegisterFeatures();
    }
}