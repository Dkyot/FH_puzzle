using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class ApiReadyManager : MonoBehaviourInitializable
    {
        protected override void Initialize()
        {
            YandexGamesManager.ApiReady();
        }
    }
}