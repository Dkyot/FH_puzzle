using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platforms.Save
{
    [Serializable]
    public class SaveInfo
    {
        [field: SerializeField] public Dictionary<int, float> LevelsScore { get; set; } = new();
        [field: SerializeField] public float MusicVolume { get; set; } = 0.5f;
        [field: SerializeField] public float SfxVolume { get; set; } = 0.5f;
        [field: SerializeField] public string Language { get; set; }
        [field: SerializeField] public long LastSaveTimeTicks { get; set; }
    }
}