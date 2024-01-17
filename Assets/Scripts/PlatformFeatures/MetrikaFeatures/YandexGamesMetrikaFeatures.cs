using UnityEngine;
using YG;

namespace PlatformFeatures.MetrikaFeatures
{
    public class YandexGamesMetrikaFeatures : MetrikaFeatures
    {
        [SerializeField] private InfoYG infoYg;

        protected override void Init()
        {
        }

        public override void SendGameReady()
        {
            if (infoYg.autoGameReadyAPI) return;
            YandexGame.GameReadyAPI();
        }

        public override void SendEvent(string eventName)
        {
            YandexMetrica.Send(eventName);
        }

        public override void SendEvent(MetrikaEventEnum eventName)
        {
            YandexMetrica.Send(eventName.ToString());
        }
    }
}