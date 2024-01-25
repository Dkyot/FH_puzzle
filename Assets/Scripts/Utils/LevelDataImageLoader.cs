using FH.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public sealed class LevelDataImageLoader
    {
        public Awaitable<List<Sprite>> LoadLevelsImages(List<LevelDataSO> levelData) => WhenAll(levelData.Select(l => l.GetLevelImageAsync()).ToArray());

        private static Awaitable<List<T>> WhenAll<T>(params Awaitable<T>[] awaitables) {
            var completionSource = new AwaitableCompletionSource<List<T>>();
            var result = new List<T>(awaitables.Length);
            var lenght = awaitables.Length;
            var count = lenght;

            for (var i = 0; i < lenght; i++)
            {
                StartAwaitableAndInvokeCallback(awaitables[i], i, OnAwaitableCompleted);
            }

            void OnAwaitableCompleted(int awaitableIndex, T awaitableResult)
            {
                count--;
                result.Add(awaitableResult);
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
