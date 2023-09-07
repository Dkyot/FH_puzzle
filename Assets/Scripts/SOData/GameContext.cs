using UnityEngine;

namespace FH.SO {
    [CreateAssetMenu(fileName = "GameContext", menuName = "SOData/GameContext")]
    public class GameContext : ScriptableObject {
        [field: SerializeField] public LevelsDataBaseSO LevelDataBase { get; private set; }
        [field: SerializeField] public SceneManagerProxy SceneManagerProxy { get; private set; }
        [field: SerializeField] public LevelContext LevelContext { get; private set; }
    }
}
