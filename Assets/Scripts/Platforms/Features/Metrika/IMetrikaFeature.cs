namespace Platforms.Metrika
{
    public interface IMetrikaFeature
    {
        void SendGameReady();
        void SendEvent(string eventName);
        void SendEvent(MetrikaEventEnum eventName);
    }
}