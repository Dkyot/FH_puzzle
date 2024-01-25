using System.Collections.Generic;
using UnityEngine;

namespace FH.SO {
    [CreateAssetMenu(fileName = "LevelsDtaBase", menuName = "SOData/LevelsDataBase")]
    public sealed class LevelsDataBaseSO : ScriptableObject {
        public IEnumerable<AddressableLevelDataSO> Levels => _levels;
        [SerializeField] private AddressableLevelDataSO[] _levels;
    }
}
