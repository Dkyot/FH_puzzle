using UnityEngine;

namespace FH.UI {
    public abstract class ViewController : MonoBehaviour {
        public IScreenController ScreenController {
            get => _screenController;
            set {
                _screenController = value; 
                OnScreenControllerSet();
            }

        }

        private IScreenController _screenController;
        
        public abstract void ShowView();
        public abstract void HideView();

        protected abstract void OnScreenControllerSet();
    }
}