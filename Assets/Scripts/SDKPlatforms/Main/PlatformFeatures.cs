using PlatformsSdk.AdFeatures;
using PlatformsSdk.SaveFeatures;

namespace PlatformsSdk.Main
{
    public static class PlatformFeatures
    {
        private static IAdFeature _adFeature;
        private static ISaveFeature _saveFeature;

        public static IAdFeature Ad => _adFeature ??= new UnityAdFeature();
        public static ISaveFeature Save => _saveFeature ??= new UnitySaveFeature();
        
        public static void Configure(IAdFeature adFeature, ISaveFeature saveFeature)
        {
            _adFeature = adFeature;
            _saveFeature = saveFeature;
        }
    }
}