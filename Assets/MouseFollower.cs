using FH.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour {
    [SerializeField] private PlayerInputHandler _input;

    private Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    private void Update() {
        var mousePosition = _input.MousePosition;
        var wordPosition = _camera.ScreenToWorldPoint(mousePosition);
        wordPosition.z = 0;

        transform.position = wordPosition;
    }
}
