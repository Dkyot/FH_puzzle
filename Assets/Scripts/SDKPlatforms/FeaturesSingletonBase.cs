using UnityEngine;

namespace PlatformsSdk
{
    public abstract class FeaturesSingletonBase<T> : MonoBehaviour where T : FeaturesSingletonBase<T>
    {
        public static T Instance { get; private set; }
        
        protected virtual void Awake()
        {
            Init();
            Instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }

        protected abstract void Init();
    }
}