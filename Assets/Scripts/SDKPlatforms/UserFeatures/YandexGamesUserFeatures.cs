using UnityEngine;
using YG;

namespace PlatformsSdk.UserFeatures
{
    public class YandexGamesUserFeatures : UserFeatures
    {
        [SerializeField] private InfoYG infoYg;
        [SerializeField] private string mainLeaderboardName;

        public override bool CanReviewGame()
        {
            return YandexGame.EnvironmentData.reviewCanShow;
        }

        public override void OpenReviewGame()
        {
            if (CanReviewGame())
            {
                YandexGame.ReviewShow(true);
            }
        }

        public override void SetMainLeaderboardScore(int value)
        {
            SetLeaderboardScore(mainLeaderboardName, value);
        }

        public override void SetLeaderboardScore(string leaderboardName, int value)
        {
            if (infoYg.leaderboardEnable)
            {
                YandexGame.NewLeaderboardScores(leaderboardName, value);
            }
        }
    }
}