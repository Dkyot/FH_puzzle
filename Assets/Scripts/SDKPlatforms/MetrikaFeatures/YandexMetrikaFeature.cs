using YG;

namespace PlatformsSdk.MetrikaFeatures
{
    public class YandexMetrikaFeature : IMetrikaFeature
    {
        private readonly InfoYG _infoYg;
        
        public YandexMetrikaFeature(InfoYG infoYg)
        {
            _infoYg = infoYg;
        }
        
        public void SendGameReady()
        {
            if (_infoYg.autoGameReadyAPI) return;
            YandexGame.GameReadyAPI();
        }

        public void SendEvent(string eventName)
        {
            if (_infoYg.metricaEnable)
            {
                YandexMetrica.Send(eventName);
            }
        }

        public void SendEvent(MetrikaEventEnum eventName)
        {
            if (_infoYg.metricaEnable)
            {
                YandexMetrica.Send(eventName.ToString());
            }
        }
    }
}