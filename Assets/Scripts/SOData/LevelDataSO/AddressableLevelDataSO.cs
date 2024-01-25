using FH.Utils;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FH.SO
{
    [CreateAssetMenu(fileName = "AddressableLevelData_", menuName = "SOData/Adressable Level Data")]
    public sealed class AddressableLevelDataSO : LevelDataSO
    {
        [field: SerializeField] public AssetReferenceSprite LevelImage { get; private set; }

        public override async Awaitable<Sprite> GetLevelImageAsync()
        {
            var levelImageRef = LevelImage;
            AsyncOperationHandle<Sprite> loadOperation;

            loadOperation = await levelImageRef.LoadAssetAsync().CompleteAsync();
            if (loadOperation.Status == AsyncOperationStatus.Failed)throw loadOperation.OperationException;

            var result = loadOperation.Result;
            return result != null ? result : throw new Exception($"Loaded image is null: {levelImageRef.AssetGUID}");
        }

        public override void ReleaseImage()
        {
            var levelImageRef = LevelImage;
            levelImageRef.ReleaseAsset();
        }
    }
}
