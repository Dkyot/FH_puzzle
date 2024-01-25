using FH.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public sealed class LevelDataImageLoader
    {
        public Awaitable<Sprite[]> LoadLevelsImages(LevelDataSO[] levelData) => WhenAll(levelData.Select(l => l.GetLevelImageAsync()).ToArray());

        private static Awaitable<T[]> WhenAll<T>(params Awaitable<T>[] awaitables) {
            var completionSource = new AwaitableCompletionSource<T[]>();
            var count = awaitables.Length;

            var result = new T[awaitables.Length];

            for (var i = 0; i < awaitables.Length; i++)
            {
                StartAwaitableAndInvokeCallback(awaitables[i], i, OnAwaitableCompleted);
            }

            void OnAwaitableCompleted(int awaitableIndex, T awaitableResult)
            {
                count--;
                result[awaitableIndex] = awaitableResult;
                if (count <= 0) completionSource.TrySetResult(result);
            }

            return completionSource.Awaitable;
        }

        private static async void StartAwaitableAndInvokeCallback<T>(Awaitable<T> awaitable, int awaitableIndex, Action<int, T> callback) {
            var result = await awaitable;
            callback.Invoke(awaitableIndex, result);
        }
    }
}
