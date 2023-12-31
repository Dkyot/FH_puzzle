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

        public int AdFindPairBonus {
            set => _adFindPairBonus.text = $"+{value}";
        }

        public int AdPeekBonus {
            set => _adPeekBonus.text = $"+{value}";
        }

        public int PeekUsageCount {
            set {
                if (value <= 0) {
                    _peekUsageCounter.style.display = DisplayStyle.None;
                    _peekAdIcon.style.display = DisplayStyle.Flex;
                }
                else {
                    _peekUsageCounter.style.display = DisplayStyle.Flex;
                    _peekAdIcon.style.display = DisplayStyle.None;

                    _peekUsageCounter.text = $"{value}X";
                }
            }
        }

        public int FindPairUsageCount {
            set {
                if (value <= 0) {
                    _findPairUsageCounter.style.display = DisplayStyle.None;
                    _findPairAdIcon.style.display = DisplayStyle.Flex;
                }
                else {
                    _findPairUsageCounter.style.display = DisplayStyle.Flex;
                    _findPairAdIcon.style.display = DisplayStyle.None;

                    _findPairUsageCounter.text = $"{value}X";
                }
            }
        }

        private Button _pauseButton;
        private Button _resetButton;
        private Button _peekButton;
        private Button _findPairButton;

        private Label _findPairUsageCounter;
        private Label _peekUsageCounter;

        private Label _adPeekBonus;
        private Label _adFindPairBonus;

        private Label _mistakesLabel;
        private Label _timeLabel;

        private VisualElement _findPairAdIcon;
        private VisualElement _peekAdIcon;

        private ElementPointer _pairPointer;
        private ElementPointer _peekPointer;

        public void ShowTipsPointers() {
            _pairPointer.StartAnimation();
            _peekPointer.StartAnimation();
        }

        public void HideTipsPointers()
        {
            _pairPointer.StopAnimation();
            _peekPointer.StopAnimation();
        }

        protected override void OnInit() {
            _pauseButton = this.Q<Button>("PauseButton");
            _resetButton = this.Q<Button>("ResetButton");
            _peekButton = this.Q<Button>("PeekButton");
            _findPairButton = this.Q<Button>("FindPairButton");

            _timeLabel = this.Q<Label>("Timer");
            _mistakesLabel = this.Q<Label>("Mistakes");

            _findPairUsageCounter = this.Q<Label>("FindPairUseCounterLabel");
            _peekUsageCounter = this.Q<Label>("PeekUseCounterLabel");

            _findPairAdIcon = this.Q<VisualElement>("FindPairAdIcon");
            _peekAdIcon = this.Q<VisualElement>("PeekAdIcon");

            _pairPointer = this.Q<ElementPointer>("ElementPointerPair");
            _peekPointer = this.Q<ElementPointer>("ElementPointerPeek");

            _adFindPairBonus = this.Q<Label>("AdFindPairBonusLabel");
            _adPeekBonus = this.Q<Label>("AdPeekBonusLabel");

            _peekButton.clicked += InvokeButtonPressed;
            _resetButton.clicked += InvokeButtonPressed;
            _pauseButton.clicked += InvokeButtonPressed;
            _findPairButton.clicked += InvokeButtonPressed;

            _pauseButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
            _resetButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
            _findPairButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
            _peekButton.RegisterCallback<MouseEnterEvent>(OnButtonHovered);
        }

        public new sealed class UxmlFactory : UxmlFactory<GameUIView, UxmlTraits> { }
    }
}
