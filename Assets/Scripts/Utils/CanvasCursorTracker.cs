using FH.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UI;

namespace FH.Utils {
    [RequireComponent(typeof(RectTransform))]
    public sealed class CanvasCursorTracker : MonoBehaviour {
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private Canvas parent;

        private RectTransform rect;

        private void Awake() {
            rect = GetComponent<RectTransform>();
        }

        void Update() {
            var position = inputHandler.MousePosition / parent.scaleFactor;
            rect.anchoredPosition = position;
        }
    }
}
