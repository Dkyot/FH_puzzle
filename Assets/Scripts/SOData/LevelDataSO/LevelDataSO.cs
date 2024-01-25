using System;
using UnityEngine;

namespace FH.SO
{
    public abstract class LevelDataSO : ScriptableObject
    {
        [field: SerializeField] public LevelParams Params { get; private set; }

        [NonSerialized]
        public int number;

        //Data that is stored outside
        public float score;
        public bool isCompleted;

        public abstract Awaitable<Sprite> GetLevelImageAsync();
        public abstract void ReleaseImage();
    }
}
