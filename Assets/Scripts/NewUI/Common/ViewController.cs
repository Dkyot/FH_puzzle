using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI {
    public abstract class ViewController : MonoBehaviour {
        private IScreenController _screenController;

        public IScreenController ScreenController {
            get => _screenController;
            set {
                _screenController = value;
                OnScreenControllerSet();
            }

        }
        public abstract void ShowView();
        public abstract void HideView();
        protected abstract void OnScreenControllerSet();
    }

    public abstract class ViewController<T> : ViewController where T : ViewBase {
        protected T view;

        public override void HideView() {
            view.Hide();
        }

        public override void ShowView() {
            view.Show();
        }

        protected override void OnScreenControllerSet() {
            view = ScreenController.Document.rootVisualElement.Q<T>();
            view.Init();
        }
    }
}