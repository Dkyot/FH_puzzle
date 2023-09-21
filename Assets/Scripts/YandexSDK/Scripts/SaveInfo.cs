using System;
using System.Collections.Generic;
using SkibidiRunner.Managers;
using UnityEngine;

namespace YandexSDK.Scripts
{
    /// <summary>
    /// Data that is downloaded and stored from outside, such as Yandex Games.
    /// </summary>
    [Serializable]
    public class SaveInfo
    {
        [field: SerializeField] public List<float> LevelsScore { get; set; } = new();
        [field: SerializeField] public float MasterSoundVolume { get; set; } = 1;
        [field: SerializeField] public float MusicVolume { get; set; } = 1;
        [field: SerializeField] public float SfxVolume { get; set; } = 1;
        [field: SerializeField] public long LastSaveTimeTicks { get; set; }
    }
}