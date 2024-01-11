using System;
using UnityEngine;

namespace FH.Inputs {
    public class PlayerInputHandler : MonoBehaviour {
        public event Action<Vector2> Pressed;

        public Vector2 MousePosition => Input.mousePosition;

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Pressed?.Invoke(MousePosition);
            }
        }
    }
}
