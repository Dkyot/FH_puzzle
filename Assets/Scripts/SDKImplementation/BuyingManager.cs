using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class BuyingManager : MonoBehaviour
    {
        [SerializeField, Space] private UnityEvent buyItem;

        public void BuyForAd(int result)
        {
            switch (result)
            {
                case 0:
                    PauseManager.Instance.PauseGame();
                    break;
                case 1:
                    buyItem?.Invoke();
                    break;
                case 2:
                    PauseManager.Instance.ResumeGame();
                    break;
            }
        }
    }
}