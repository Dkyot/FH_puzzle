using System;

namespace PlatformsSdk.UserFeatures
{
    public abstract class UserFeatures : FeaturesSingletonBase<UserFeatures>
    {
        protected override void Init()
        {
            
        }

        public abstract bool CanReviewGame();
        public abstract void OpenReviewGame();
        public abstract void SetMainLeaderboardScore(int value);
        public abstract void SetLeaderboardScore(string leaderboardName, int value);

    }
}