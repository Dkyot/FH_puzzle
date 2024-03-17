using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Platforms.Addressables
{
    [CreateAssetMenu(fileName = "_Addressables", menuName = "Platform/Addressables")]
    public class PlatformAddressablesSo: ScriptableObject
    {
        [field:SerializeField] public List<AddressableAssetGroup> Addressables { get; private set; }
    }
}