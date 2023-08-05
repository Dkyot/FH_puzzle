using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Utils {
    public class ScrollingBgTextureController : MonoBehaviour {
        [SerializeField] private Camera _renderCamera;

        private void Awake() {
            _renderCamera.enabled = false;
        }

        public void EnableRendering() {
            _renderCamera.enabled = true;
        }

        public void DisableRendering() {
            _renderCamera.enabled = false;
        }
    }
}