using YG;

namespace Platforms.Metrika
{
    public class YandexGamesMetrikaFeature : IMetrikaFeature
    {
        public void SendGameReady()
        {
            if (YG2.infoYG.Basic.autoGRA) return;
            YG2.GameReadyAPI();
        }

        public void SendEvent(string eventName)
        {
            if (YG2.infoYG.Metrica.enable)
            {
                YG2.MetricaSend(eventName);
            }
        }

        public void SendEvent(MetrikaEventEnum eventName)
        {
            if (YG2.infoYG.Metrica.enable)
            {
                YG2.MetricaSend(eventName.ToString());
            }
        }
    }
}