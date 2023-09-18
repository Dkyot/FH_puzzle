using System;
using System.Threading.Tasks;
using UnityEngine;

namespace YandexSDK.Scripts
{
    public class LocalYandexData
    {
        private static LocalYandexData _instance;
        public static LocalYandexData Instance => _instance ??= new LocalYandexData();
        
        public bool YandexDataLoaded { get; private set; }
        
        public event Action OnYandexDataLoaded;

        public SaveInfo SaveInfo { get; private set; }

        private LocalYandexData()
        {
            SaveInfo = new SaveInfo();
        }
        
        public void SetPlayerData(SaveInfo playerData)
        {
            YandexDataLoaded = true;
            if (playerData.LastSaveTimeTicks != 0)
            {
                SaveInfo = playerData;
            }
            OnYandexDataLoaded?.Invoke();
        }
        
        public void DebugSetPlayerData(SaveInfo playerData)
        {
            YandexDataLoaded = true;
            SaveInfo = playerData;
        }

        public void SaveData()
        {
            SaveInfo.LastSaveTimeTicks = DateTime.UtcNow.Ticks;
            YandexGamesManager.SavePlayerData(SaveInfo);
            Debug.Log("Data save");
        }

        public void ResetProgress()
        {
            SaveInfo = new SaveInfo();
            YandexGamesManager.SavePlayerData(SaveInfo);
            OnYandexDataLoaded?.Invoke();
        }
    }
}