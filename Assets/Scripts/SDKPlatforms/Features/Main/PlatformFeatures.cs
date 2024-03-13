using SDKPlatforms.Ad;
using SDKPlatforms.Metrika;
using SDKPlatforms.Save;
using SDKPlatforms.User;

namespace SDKPlatforms.Main
{
    public static class PlatformFeatures
    {
        private static IAdFeature _adFeature;
        private static ISaveFeature _saveFeature;
        private static IUserFeature _userFeature;
        private static IMetrikaFeature _metrikaFeature;

        public static IAdFeature Ad => _adFeature ??= new UnityAdFeature();
        public static ISaveFeature Save => _saveFeature ??= new UnitySaveFeature();
        public static IUserFeature User => _userFeature ??= new UnityUserFeature();
        public static IMetrikaFeature Metrika => _metrikaFeature ??= new UnityMetrikaFeature();

        public static void Configure(IAdFeature adFeature, ISaveFeature saveFeature, IUserFeature userFeature = null,
            IMetrikaFeature metrikaFeature = null)
        {
            _adFeature = adFeature;
            _saveFeature = saveFeature;
            _userFeature = userFeature;
            _metrikaFeature = metrikaFeature;
        }
    }
}