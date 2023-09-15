using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.GameUI {
    public sealed class GameUIView : ViewBase {
        public event Action PausePressed {
            add => _pauseButton.clicked += value;
            remove => _pauseButton.clicked -= value;
        }

        public event Action ResetPressed {
            add => _resetButton.clicked += value;
            remove => _resetButton.clicked -= value;
        }

        public event Action FindPairPressed {
            add => _findPairButton.clicked += value;
            remove => _findPairButton.clicked -= value;
        }

        public event Action PeekPressed {
            add => _peekButton.clicked += value;
            remove => _peekButton.clicked -= value;
        }
        
        public string MistakesText {
            set => _mistakesLabel.text = value;
        }

        public string TimeText {
            set => _timeLabel.text = value;
        }

        private Button _pauseButton;
        private Button _resetButton;
        private Button _peekButton;
        private Button _findPairButton;

        private Label _mistakesLabel;
        private Label _timeLabel;

        protected override void OnInit() {
            _pauseButton = this.Q<Button>("PauseButton");
            _resetButton = this.Q<Button>("ResetButton");
            _peekButton = this.Q<Button>("PeekButton");
            _findPairButton = this.Q<Button>("FindPairButton");

            _timeLabel = this.Q<Label>("Timer");
            _mistakesLabel = this.Q<Label>("Mistakes");
        }

        public new sealed class UxmlFactory : UxmlFactory<GameUIView, UxmlTraits> { }
    }
}
