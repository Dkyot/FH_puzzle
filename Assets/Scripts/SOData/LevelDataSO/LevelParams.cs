using System;
using FH.Cards;
using UnityEngine;

namespace FH.SO
{
    [Serializable]
    public sealed class LevelParams
    {
        [field: SerializeField] public ColorsSO Palete { get; private set; }
        [field: SerializeField, Range(2, 8)] public int Rows { get; private set; }
        [field: SerializeField, Range(2, 8)] public int Columns { get; private set; }
        [field: SerializeField] public LevelType LevelType { get; private set; }
    }

}
