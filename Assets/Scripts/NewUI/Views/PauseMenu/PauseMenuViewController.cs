using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FH.UI.Views.PauseMenu {
    public sealed class PauseMenuViewController : ViewController<PauseMenuView> {
        [SerializeField] private UnityEvent _resumePressed;
        [SerializeField] private UnityEvent _exitPressed;

        protected override void OnScreenControllerSet() {
            base.OnScreenControllerSet();
            view.ContinePressed += OnResumePressed;
            view.ToMainMenuPressed += OnExitPressed;
        }

        private void OnDisable() {
            view.ContinePressed -= OnResumePressed;
            view.ToMainMenuPressed -= OnExitPressed;
        }

        private void OnResumePressed() {
            _resumePressed?.Invoke();
        }

        private void OnExitPressed() {
            _exitPressed?.Invoke();
        }
    }
}
