using FH.Utils;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FH.SO {
    [CreateAssetMenu(fileName = "LevelData_", menuName = "SOData/LevelData")]
    public sealed class LevelDataSO : ScriptableObject {
        [field: SerializeField] public LevelParams Params { get; private set; }
        [field: SerializeField] public LocalizableString CharacterName { get; private set; }
        [field: SerializeField] public AssetReferenceSprite LevelImage { get; private set; }

        [NonSerialized]
        public int number;

        //Data that is stored outside
        public float score;
        public bool isCompleted;

        [Serializable]
        public sealed class LevelParams {
            [field: SerializeField] public ColorsSO Palete { get; private set; }
            [field: SerializeField, Range(4, 6)] public int Rows { get; private set; }
            [field: SerializeField, Range(4, 6)] public int Columns { get; private set; }
        }
    }

}
