using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FH.Inputs {
    public class PlayerInputHandler : MonoBehaviour {
        public event Action<Vector2> Pressed;

        public Vector2 MousePosition => _input.InGame.Position.ReadValue<Vector2>();

        private Controls _input;

        private void Awake() {
            _input = new Controls();
        }

        private void OnClick(InputAction.CallbackContext context) {
            Pressed?.Invoke(_input.InGame.Position.ReadValue<Vector2>());
        }

        private void OnEnable() {
            _input.Enable();
            _input.InGame.LMB_click.performed += OnClick;
        }

        private void OnDisable() {
            _input.Disable();
            _input.InGame.LMB_click.performed -= OnClick;
        }
    }
}
