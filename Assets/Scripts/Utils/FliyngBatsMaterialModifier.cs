using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Utils {
    [ExecuteAlways]
    public class FliyngBatsMaterialModifier : MonoBehaviour {
        private const string _framePropertyName = "_Frame";

        public int MaterialFrameIndex {
            get => _material.GetInt(_framePropertyName);
            set => _material.SetInt(_framePropertyName, value);
        }

        public int frameIndex = 0;

        [SerializeField] private Material _material;
        private int _lastUpdatedValue;

        private void Awake() {
            MaterialFrameIndex = 0;
            frameIndex = 0;
            _lastUpdatedValue = 0;
        }

        private void Update() {
            if (_lastUpdatedValue != frameIndex) {
                MaterialFrameIndex = frameIndex;
                _lastUpdatedValue = frameIndex;
            }
        }

        private void OnDisable() {
            MaterialFrameIndex = 0;
            _lastUpdatedValue = 0;
            frameIndex = 0;
        }
    }
}
