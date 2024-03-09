using PlatformsSdk.AdFeatures;
using PlatformsSdk.SaveFeatures;
using PlatformsSdk.UserFeatures;

namespace PlatformsSdk.Main
{
    public static class PlatformFeatures
    {
        private static IAdFeature _adFeature;
        private static ISaveFeature _saveFeature;
        private static IUserFeature _userFeature;

        public static IAdFeature Ad => _adFeature ??= new UnityAdFeature();
        public static ISaveFeature Save => _saveFeature ??= new UnitySaveFeature();
        public static IUserFeature User => _userFeature ??= new UnityUserFeature();
        
        public static void Configure(IAdFeature adFeature, ISaveFeature saveFeature, IUserFeature userFeature)
        {
            _adFeature = adFeature;
            _saveFeature = saveFeature;
            _userFeature = userFeature;
        }
    }
}