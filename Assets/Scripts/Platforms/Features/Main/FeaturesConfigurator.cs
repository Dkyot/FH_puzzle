using Platforms.Features;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platforms.Main
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