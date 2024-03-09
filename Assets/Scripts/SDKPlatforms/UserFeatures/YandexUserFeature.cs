using YG;

namespace PlatformsSdk.UserFeatures
{
    public class YandexUserFeature : IUserFeature
    {
        private readonly InfoYG _infoYg;
        private readonly string _mainLeaderboardName;

        public YandexUserFeature(InfoYG infoYg, string mainLeaderboardName)
        {
            _infoYg = infoYg;
            _mainLeaderboardName = mainLeaderboardName;
        }
        
        public bool CanReviewGame()
        {
            return YandexGame.EnvironmentData.reviewCanShow;
        }

        public void OpenReviewGame()
        {
            if (CanReviewGame())
            {
                YandexGame.ReviewShow(true);
            }
        }

        public void SetMainLeaderboardScore(int value)
        {
            SetLeaderboardScore(_mainLeaderboardName, value);
        }

        public void SetLeaderboardScore(string leaderboardName, int value)
        {
            if (_infoYg.leaderboardEnable)
            {
                YandexGame.NewLeaderboardScores(leaderboardName, value);
            }
        }
    }
}