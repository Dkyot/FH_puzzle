using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FH.Utils {
    public static class AsyncOperationHandleExtension {
        public static async Awaitable<AsyncOperationHandle<T>> CompleteAsync<T>(this AsyncOperationHandle<T> operation) {
            while (!operation.IsDone) {
                await Awaitable.NextFrameAsync();
            }

            return operation;
        }
    }
}
