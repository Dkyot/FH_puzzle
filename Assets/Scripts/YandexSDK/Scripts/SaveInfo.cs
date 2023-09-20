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
        [field: SerializeField] public List<LevelDataOutside> Levels { get; set; } = new();
        [field: SerializeField] public long LastSaveTimeTicks { get; set; }
    }
}