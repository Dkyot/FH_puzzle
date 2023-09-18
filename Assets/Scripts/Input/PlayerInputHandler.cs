using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FH.Inputs {
    public class PlayerInputHandler : MonoBehaviour {
        public event Action<Vector2> Pressed;

        private Controls input;

        private void Awake() {
            input = new Controls();
        }

        private void OnClick(InputAction.CallbackContext context) {
            Pressed?.Invoke(input.InGame.Position.ReadValue<Vector2>());
        }

        private void OnTouch(InputAction.CallbackContext context) {
            // Todo Need Testing
            Pressed?.Invoke(input.InGame.ScreenPressed.ReadValue<Vector2>());
        }

        #region InputSystem subscriptions
        private void OnEnable() {
            input.Enable();
            input.InGame.LMB_click.performed += OnClick;
            input.InGame.ScreenPressed.performed += OnTouch;
        }

        private void OnDisable() {
            input.Disable();
            input.InGame.LMB_click.performed -= OnClick;
            input.InGame.ScreenPressed.performed -= OnTouch;
        }
        #endregion
    }
}
