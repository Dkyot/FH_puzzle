using FH.Sound;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI {
    [RequireComponent(typeof(UIDocument))]
    public sealed class ScreenController : MonoBehaviour, IScreenController {
        public UIDocument Document => _document;

        [Header("Views")]
        [SerializeField] private ViewController _initialView;
        [SerializeField] private List<ViewController> _controllers;

        [Header("Sounds")]
        [SerializeField] private AudioClip _buttonHoverSound;
        [SerializeField] private AudioClip _buttonPressedSound;

        private const float _hoverSoundScale = 1f;
        private const float _pressSoundScale = 0.6f;

        private UIDocument _document;

        public void ShowView(ViewController view) {
            for (var o = 0; o < _controllers.Count; o++) {
                var v = _controllers[o];
                if (v == view)
                    continue;
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

                controller.ButtonHovered += OnButtonHovered;
                controller.ButtonPressed += OnButtonPressed;
            }
        }

        private void OnButtonHovered() {
            SoundManager.Instance.PlayOneShot(_buttonHoverSound, _hoverSoundScale);
        }

        private void OnButtonPressed() {
            SoundManager.Instance.PlayOneShot(_buttonPressedSound, _pressSoundScale);
        }
    }
}