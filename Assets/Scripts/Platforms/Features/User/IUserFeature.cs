﻿namespace Platforms.User
{
    public interface IUserFeature
    {
        bool CanReviewGame();
        void OpenReviewGame();
        void SetMainLeaderboardScore(int value);
        void SetLeaderboardScore(string leaderboardName, int value);
    }
}