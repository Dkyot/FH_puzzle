using System;
using FH.Cards;
using FH.Inputs;
using FH.UI.Views.GameUI;
using UnityEngine;
using UnityEngine.Serialization;

namespace FH.Level
{
    public class TipsPointerController : MonoBehaviour
    {
        [SerializeField] private GameUIViewController gameUIController;
        [SerializeField] private PlayerInputHandler playerInputHandler;

        private void OnEnable()
        {
            playerInputHandler.Pressed += OnGamePressed;
        }

        private void OnDisable()
        {
            playerInputHandler.Pressed -= OnGamePressed;
        }

        private void OnGamePressed(Vector2 obj)
        {
            gameUIController.HideTipsPointer();
        }

        public void ShowTipsPointer()
        {
            gameUIController.ShowTipsPointer();
        }
    }
}