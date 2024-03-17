using Platforms.Main;
using UnityEngine;

namespace Platforms.Save
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