﻿
using System.Collections.Generic;
using PlatformFeatures.SaveFeatures;
using UnityEngine.Serialization;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public SaveInfo saveInfo = new();
    }
}