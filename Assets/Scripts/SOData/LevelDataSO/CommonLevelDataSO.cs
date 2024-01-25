using UnityEngine;

namespace FH.SO
{
    [CreateAssetMenu(fileName = "CommonLevelData_", menuName = "SOData/LevelData")]
    public sealed class CommonLevelDataSO : LevelDataSO
    {
        [SerializeField] private Sprite _levelImage;

        public override async Awaitable<Sprite> GetLevelImageAsync() => _levelImage;
        public override void ReleaseImage() { }
    }
}
