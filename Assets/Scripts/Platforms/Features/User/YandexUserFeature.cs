using YG;

namespace Platforms.User
{
    public class YandexUserFeature : IUserFeature
    {
        private readonly string _mainLeaderboardName;

        public YandexUserFeature(string mainLeaderboardName)
        {
            _mainLeaderboardName = mainLeaderboardName;
        }
        
        public bool CanReviewGame()
        {
            return YG2.reviewCanShow;
        }

        public void OpenReviewGame()
        {
            if (CanReviewGame())
            {
                YG2.ReviewShow();
            }
        }

        public void SetMainLeaderboardScore(int value)
        {
            SetLeaderboardScore(_mainLeaderboardName, value);
        }

        public void SetLeaderboardScore(string leaderboardName, int value)
        {
            if (YG2.infoYG.Leaderboards.enable)
            {
                YG2.SetLeaderboard(leaderboardName, value);
            }
        }
    }
}