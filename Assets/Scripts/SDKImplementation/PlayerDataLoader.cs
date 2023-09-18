using System.Threading.Tasks;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class PlayerDataLoader : MonoBehaviourInitializable
    {
        protected override void Initialize()
        {
            if(LocalYandexData.Instance.YandexDataLoaded) return;
            YandexGamesManager.LoadPlayerData(gameObject, nameof(OnPlayerDataReceived));
        }

        public void OnPlayerDataReceived(string json)
        {
            //Debug.Log("OnPlayerDataReceived " + json);
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Failed to load player data");
                Initialize();
            }
            else
            {
                Debug.Log("Data loaded");
                LocalYandexData.Instance.SetPlayerData(JsonUtility.FromJson<SaveInfo>(json));
            }
        }
    }
}