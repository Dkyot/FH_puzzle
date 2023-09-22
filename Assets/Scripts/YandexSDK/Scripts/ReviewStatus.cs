namespace YandexSDK.Scripts
{
    public enum ReviewStatus
    {
        Unknown = -1,
        CanReview = 0,
        NoAuth,
        GameRated,
        ReviewAlreadyRequested,
        ReviewWasRequested
    }
}