using System;
using System.Collections.Generic;
using UnityEngine;

namespace YandexSDK.Scripts
{
    
    /// <summary>
    /// Data that is downloaded and stored from outside, such as Yandex Games.
    /// </summary>
    [Serializable]
    public class SaveInfo
    {
        [field: SerializeField] public long LastSaveTimeTicks { get; set; }
    }
}