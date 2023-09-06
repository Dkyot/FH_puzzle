using FH.Scenes;
using System;
using UnityEngine;

namespace FH.SO {
    [CreateAssetMenu(fileName = "SceneManagerProxy", menuName = "SOData/SceneManagerProxy")]
    public sealed class SceneManagerProxy : ScriptableObject {
        public event Action SceneControllerSet;

        public event Action MainMenuTransitionRequested;
        public event Action LevelTransitionRequested;

        public bool IsManaged { get; set; }

        public ISceneController SceneController {
            get => _sceneController;
            set {
                _sceneController = value;
                OnControllerSet();
            }
        }

        private ISceneController _sceneController;

        private void OnControllerSet() {
            if (SceneController == null)
                return;

            if (IsManaged) {
                SceneControllerSet?.Invoke();
                return;
            }

            _ = PreloadScene();
        }

        private async Awaitable PreloadScene() {
            Debug.Log("Start preloading scene");
            await _sceneController.StartPreloading();
            Debug.Log("Start sscene");
            _sceneController.StartScene();
        }
    }
}
