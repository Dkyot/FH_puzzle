namespace Platforms.User
{
    public class UnityUserFeature : IUserFeature
    {
        public bool CanReviewGame()
        {
            return true;
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