using System;
using UnityEngine;

namespace SkibidiRunner.Managers
{
    [Serializable]
    public class LevelDataOutside
    {
        [field: SerializeField] public float Score { get; set; }
        [field: SerializeField] public bool IsCompleted { get; set; }
    }
}