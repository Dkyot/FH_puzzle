namespace SDKPlatforms.User
{
    public class UnityUserFeature : IUserFeature
    {
        public bool CanReviewGame()
        {
            return false;
        }

        public void OpenReviewGame()
        {
        }

        public void SetMainLeaderboardScore(int value)
        {
        }

        public void SetLeaderboardScore(string leaderboardName, int value)
        {
        }
    }
}