using SDKPlatforms.Features;
using UnityEngine;

namespace SDKPlatforms.Main
{
    public class FeaturesConfigurator : MonoBehaviour
    {
        public FeaturesSoBase featuresSo;
        
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