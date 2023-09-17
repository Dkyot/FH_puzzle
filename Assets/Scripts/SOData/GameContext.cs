using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace FH.SO {
    [CreateAssetMenu(fileName = "GameContext", menuName = "SOData/GameContext")]
    public class GameContext : ScriptableObject {
        [field: SerializeField] public LevelDataSO CurrentLevel { get; set; }
        [field: SerializeField] public LevelsDataBaseSO LevelDataBase { get; private set; }
        [field: SerializeField] public SceneManagerProxy SceneManagerProxy { get; private set; }
        [field: SerializeField] public VariablesGroupAsset GlobalGroupVariables { get; private set; }
    }
}
