using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace YandexSDK.Scripts
{
    public static class YandexGamesManager
    {
        [DllImport("__Internal")]
        private static extern string getPlayerName();

        [DllImport("__Internal")]
        private static extern string getPlayerPhotoURL();

        [DllImport("__Internal")]
        private static extern void requestReviewGame();

        [DllImport("__Internal")]
        private static extern int getReviewStatus();

        [DllImport("__Internal")]
        private static extern void savePlayerData(string data);

        [DllImport("__Internal")]
        private static extern void loadPlayerData(string objectName, string methodName);

        [DllImport("__Internal")]
        private static extern void setToLeaderboard(string lbName, int value);

        [DllImport("__Internal")]
        private static extern string getLang();

        [DllImport("__Internal")]
        private static extern void helloString(string str);

        [DllImport("__Internal")]
        private static extern void showSplashPageAdv(string objectName, string methodName);

        [DllImport("__Internal")]
        private static extern void showRewardedAdv(string objectName, string methodName);

        [DllImport("__Internal")]
        private static extern void apiReady();

        [DllImport("__Internal")]
        private static extern string deviceType();


        /// <summary>
        /// User name on the Yandex Games platform
        /// </summary>
        /// <returns>Player username, null on errors</returns>
        public static string GetPlayerName()
        {
            return getPlayerName();
        }

        /// <summary>
        /// User avatar on the Yandex platform 
        /// </summary>
        /// <returns>Avatar texture, null on errors</returns>
        public static async Task<Texture2D> GetPlayerPhoto()
        {
            var request = UnityWebRequestTexture.GetTexture(getPlayerPhotoURL());
            request.SendWebRequest();
            while (!request.isDone)
            {
                await Task.Yield();
            }

            if (request.result is not (UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError))
                return ((DownloadHandlerTexture)request.downloadHandler).texture;
            Debug.Log(request.error);
            return null;
        }

        /// <summary>
        /// Shows the game rating window when there are no errors
        /// </summary>
        public static void RequestReviewGame()
        {
            try
            {
                requestReviewGame();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Current status of the player's rating of the game
        /// </summary>
        /// <returns>
        /// Unknown - request has not been sent; an error on Yandex side
        /// CanReview - request is possible
        /// NoAuth - user is unauthorized
        /// GameRated - user has rated the game
        /// ReviewAlreadyRequested - request was already sent and user action is pending
        /// ReviewWasRequested - request already sent, user did something: put a rating or close the pop-up
        /// </returns>
        public static ReviewStatus GetReviewStatus()
        {
            try
            {
                return (ReviewStatus)getReviewStatus();
            }
            catch
            {
                return ReviewStatus.Unknown;
            }
        }

        public static void SavePlayerData(SaveInfo playerData)
        {
            try
            {
                string json = JsonConvert.SerializeObject(playerData);
                savePlayerData(json);
            }
            catch
            {
                // ignored
            }
        }

        public static void LoadPlayerData(GameObject gameObject, string methodName)
        {
#if UNITY_EDITOR
            gameObject.SendMessage(methodName, "DEBUG");
            return;
#endif
            try
            {
                loadPlayerData(gameObject.name, methodName);
            }
            catch
            {
                gameObject.SendMessage(methodName, "");
            }
        }

        public static void SetToLeaderboard(int value, string lbName = "gameScore")
        {
            try
            {
                setToLeaderboard(lbName, value);
            }
            catch
            {
                // ignored
            }
        }

        public static string GetLanguageString()
        {
            try
            {
                return getLang();
            }
            catch
            {
                return null;
            }
        }

        public static void ShowSplashAdv(GameObject gameObject, string methodName)
        {
#if UNITY_EDITOR
            gameObject.SendMessage(methodName, (object)0);
            gameObject.SendMessage(methodName, 1);
#endif
            try
            {
                showSplashPageAdv(gameObject.name, methodName);
            }
            catch
            {
                // ignored
            }
        }

        public static void ShowRewardedAdv(GameObject gameObject, string methodName)
        {
#if UNITY_EDITOR
            gameObject.SendMessage(methodName, (object)0);
            gameObject.SendMessage(methodName, 1);
            gameObject.SendMessage(methodName, 2);
#endif
            try
            {
                showRewardedAdv(gameObject.name, methodName);
            }
            catch
            {
                // ignored
            }
        }

        public static void ApiReady()
        {
            try
            {
                apiReady();
            }
            catch
            {
                // ignored
            }
        }

        public static DeviceType GetDeviceType()
        {
            try
            {
                return deviceType() switch
                {
                    "desktop" => DeviceType.Desktop,
                    "mobile" => DeviceType.Mobile,
                    "tablet" => DeviceType.Tablet,
                    "tv" => DeviceType.Tv,
                    _ => DeviceType.Desktop
                };
            }
            catch
            {
                return DeviceType.Desktop;
            }
        }
    }
}