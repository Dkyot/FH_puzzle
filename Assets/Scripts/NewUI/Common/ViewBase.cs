using System;
using UnityEngine.UIElements;

namespace FH.UI {
    public abstract class ViewBase : VisualElement {
        public event Action ButtonHovered;

        private bool _isInited = false;

        public void Init() {
            if (_isInited) return;
            _isInited = true;
            OnInit();
        }

        public virtual void Show() {
            style.display = DisplayStyle.Flex;
        }

        public virtual void Hide() {
            style.display = DisplayStyle.None;
        }

        protected abstract void OnInit();

        protected void OnButtonHovered(MouseEnterEvent e) {
            OnButtonHovered();
        }

        protected void OnButtonHovered() {
            ButtonHovered?.Invoke();
        }
    }
}