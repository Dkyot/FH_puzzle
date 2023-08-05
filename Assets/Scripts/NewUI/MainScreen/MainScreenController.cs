using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace FH.UI.MainMenu {
    [RequireComponent(typeof(UIDocument))]
    public class MainScreenController : MonoBehaviour, IScreenController {
        public UIDocument Document => _document;

        [SerializeField] private ViewController _initialView;
        [FormerlySerializedAs("_views")] [SerializeField] private List<ViewController> _controllers;
        private UIDocument _document;

        public void ShowView(ViewController view) {
            for (var o = 0; o < _controllers.Count; o++) {
                var v = _controllers[o];
                if (v == view) continue;
                v.HideView();
            }

            view.ShowView();
        }
        
        private void Awake() {
            _document = GetComponent<UIDocument>();
            SetUpControllers();
        }

        private void Start() {
            ShowView(_initialView);
        }

        private void SetUpControllers() {
            for (var o = 0; o < _controllers.Count; o++) {
                var controller = _controllers[o];
                controller.ScreenController = this;
            }
        }
    }
}