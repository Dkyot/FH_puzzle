using UnityEngine.UIElements;

namespace FH.UI {
    public abstract class ViewBase : VisualElement {
        private bool _isInited = false;

        public void Init() {
            if (_isInited) return;
            _isInited = true;
            OnInit();
        }

        public virtual void Show() {
            style.display = DisplayStyle.Flex;
            // this.BringToFront();
        }

        public virtual void Hide() {
            style.display = DisplayStyle.None;
        }

        protected abstract void OnInit();
    }
}