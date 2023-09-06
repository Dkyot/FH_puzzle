using FH.Scenes;
using FH.SO;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FH.MainMenu {
    public class MainMenuController : MonoBehaviour, ISceneController {
        [Header("System Referenses")]
        [SerializeField] private SceneManagerProxy _sceneManagerProxy;

        private void Start() {
           _sceneManagerProxy.SceneController = this;
        }

        public async Awaitable StartPreloading() {
            // Инициализацию UI
        }

        public void StartScene() {
        }

        public async Awaitable UnloadScene() {
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }
    }
}
