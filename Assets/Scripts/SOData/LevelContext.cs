using UnityEngine;

namespace FH.SO {
    [CreateAssetMenu(fileName = "LevelContext", menuName = "SOData/LevelContext")]
    public sealed class LevelContext : ScriptableObject {
        public LevelDataSO currentLevel;
        public LevelDataSO nextLevel;
    }
}
