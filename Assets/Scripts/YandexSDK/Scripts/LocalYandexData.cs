using System;
using System.Linq;
using System.Threading.Tasks;
using FH.SO;
using SkibidiRunner.Managers;
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
        }

        public void TrySaveLevelInfo(LevelDataSO level)
        {
            if (!SaveInfo.LevelsScore.TryGetValue(level.number, out float score))
            {
                Debug.Log("New level complete");
                SaveInfo.LevelsScore.Add(level.number, level.score);
            }
            else
            {
                if (level.score > score)
                {
                    Debug.Log($"New record level. Old {score}, new {level.score}");
                    SaveInfo.LevelsScore[level.number] = level.score;
                }
            }

            SaveData();
            SetToLeaderboard();
        }

        public void ResetProgress()
        {
            SaveInfo = new SaveInfo();
            YandexGamesManager.SavePlayerData(SaveInfo);
            OnYandexDataLoaded?.Invoke();
        }

        private void SetToLeaderboard()
        {
            float totalScore = SaveInfo.LevelsScore.Sum(pair => pair.Value);
            YandexGamesManager.SetToLeaderboard(Mathf.RoundToInt(totalScore));
        }
    }
}