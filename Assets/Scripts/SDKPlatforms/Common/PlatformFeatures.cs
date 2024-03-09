using PlatformsSdk.AdFeatures;

namespace PlatformsSdk.Common
{
    public static class PlatformFeatures
    {
        private static IAdFeature _adFeature;

        public static IAdFeature Ad => _adFeature ??= new UnityAdFeature();
        
        public static void Configure(IAdFeature adFeature)
        {
            _adFeature = adFeature;
        }
    }
}