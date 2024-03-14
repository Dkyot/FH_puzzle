using SDKPlatforms.Features;
using UnityEngine;
using UnityEngine.Serialization;

namespace SDKPlatforms.Main
{
    public class FeaturesConfigurator : MonoBehaviour
    {
        public PlatformFeaturesSoBase featuresSo;
        
        private void Awake()
        {
#if !UNITY_EDITOR
            RegisterFeatures();
#endif
        }

        private void RegisterFeatures()
        {
            featuresSo.RegisterFeatures();
        }
    }
}