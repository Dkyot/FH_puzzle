using PlatformsSdk.Main;
using UnityEngine;

namespace PlatformsSdk.SaveFeatures
{
    public class DebugSaveDataSetter : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private SaveInfo debugData;

        private void Awake()
        {

            if(PlatformFeatures.Save is UnitySaveFeature unity)
            {
                unity.SetDebugInfo(debugData);
            }
        }
#endif
    }
}