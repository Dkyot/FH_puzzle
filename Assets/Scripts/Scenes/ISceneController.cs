using UnityEngine;

namespace FH.Scenes {
    public interface ISceneController {
        Awaitable StartPreloading();
        void StartScene();
        Awaitable UnloadScene();
    }
}
