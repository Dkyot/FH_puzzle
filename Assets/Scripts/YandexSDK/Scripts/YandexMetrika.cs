namespace YandexSDK.Scripts
{
    public static class YandexMetrika
    {
        private const string gameStartedId = "GameStarted";
        private const string gameLoadedId = "GameLoaded";
        private const string levelStartedId = "LevelStarted";
        private const string levelLoadedId = "LevelLoaded";
        private const string levelCompletedId = "LevelCompleted";
        private const string galleryOpenedId = "GalleryOpened";
        private const string settingsOpenedId = "SettingsOpened";
        private const string pairAllUsedId = "PairAllUsed";
        private const string eyeAllUsedId = "EyeAllUsed";
        private const string pairReceivedId = "PairReceived";
        private const string eyeReceivedId = "EyeReceived";

        public static void LevelCompleted(int levelNumber)
        {
            YandexGamesManager.CallYandexMetric(levelCompletedId + levelNumber);
        }
        
        public static void GameStarted()
        {
            YandexGamesManager.CallYandexMetric(gameStartedId);
        }
        
        public static void GameLoaded()
        {
            YandexGamesManager.CallYandexMetric(gameLoadedId);
        }
        
        public static void LevelStarted()
        {
            YandexGamesManager.CallYandexMetric(levelStartedId);
        }
        
        public static void LevelLoaded()
        {
            YandexGamesManager.CallYandexMetric(levelLoadedId);
        }
        
        public static void GalleryOpened()
        {
            YandexGamesManager.CallYandexMetric(galleryOpenedId);
        }
        
        public static void SettingsOpened()
        {
            YandexGamesManager.CallYandexMetric(settingsOpenedId);
        }
        
        public static void PairAllUsed()
        {
            YandexGamesManager.CallYandexMetric(pairAllUsedId);
        }
        
        public static void EyeAllUsed()
        {
            YandexGamesManager.CallYandexMetric(eyeAllUsedId);
        }
        
        public static void PairReceived()
        {
            YandexGamesManager.CallYandexMetric(pairReceivedId);
        }
        
        public static void EyeReceived()
        {
            YandexGamesManager.CallYandexMetric(eyeReceivedId);
        }
    }
}