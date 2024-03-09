using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformsSdk.FeaturesConfigures
{
    public abstract class FeaturesConfigureBase : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_EDITOR
            RegisterFeatures();
#endif
        }
        
        protected abstract void RegisterFeatures();
    }
}