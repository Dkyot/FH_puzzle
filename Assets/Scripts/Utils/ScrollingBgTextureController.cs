using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Utils {
    public class ScrollingBgTextureController : MonoBehaviour {
        [SerializeField] private Camera _renderCamera;

        private void Awake() {
            DisableRendering();
        }

        public void EnableRendering() {
            gameObject.SetActive(true);
            _renderCamera.enabled = true;
        }

        public void DisableRendering() {
            gameObject.SetActive(true);
            _renderCamera.enabled = false;
        }
    }
}