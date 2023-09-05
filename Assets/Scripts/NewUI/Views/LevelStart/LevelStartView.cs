using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.LevelStart {
    public class LevelStartView : ViewBase {
        private const string _transition1Class = "transition";
        private VisualElement _firstCount;
        private VisualElement _secondCount;
        private VisualElement _thirdCount;

        private VisualElement _levelNameLabel;
        private VisualElement _levelStartLable;

        public override void Show() {
            base.Show();

            HideLevelName();
            HideStartLabel();

            HideFirstCount();
            HideSecondCount();
            HideThirdCount();
        }

        public void ShowLevelName() {
            _levelNameLabel.style.display = DisplayStyle.Flex;
            _levelNameLabel.RemoveFromClassList(_transition1Class);
        }

        public void HideLevelName() {
            _levelNameLabel.AddToClassList(_transition1Class);
            _levelNameLabel.style.display = DisplayStyle.None;
        }

        public void ShowStartLabel() {
            _levelStartLable.style.display = DisplayStyle.Flex;
            _levelStartLable.RemoveFromClassList(_transition1Class);
        }

        public void HideStartLabel() {
            _levelStartLable.style.display = DisplayStyle.None;
            _levelStartLable.AddToClassList(_transition1Class);
        }

        public void ShowThirdCount() {
            _thirdCount.style.display = DisplayStyle.Flex;
            _thirdCount.RemoveFromClassList(_transition1Class);
        }

        public void HideThirdCount() {
            _thirdCount.style.display = DisplayStyle.None;
            _thirdCount.AddToClassList(_transition1Class);
        }

        public void ShowSecondCount() {
            _secondCount.style.display = DisplayStyle.Flex;
            _secondCount.RemoveFromClassList(_transition1Class);
        }

        public void HideSecondCount() {
            _secondCount.style.display = DisplayStyle.None;
            _secondCount.AddToClassList(_transition1Class);
        }

        public void ShowFirstCount() {
            _firstCount.style.display = DisplayStyle.Flex;
            _firstCount.RemoveFromClassList(_transition1Class);
        }

        public void HideFirstCount() {
            _firstCount.style.display = DisplayStyle.None;
            _firstCount.AddToClassList(_transition1Class);
        }

        protected override void OnInit() {
            _firstCount = this.Q("1");
            _firstCount.AddToClassList(_transition1Class);

            _secondCount = this.Q("2");
            _secondCount.AddToClassList(_transition1Class);

            _thirdCount = this.Q("3");
            _thirdCount.AddToClassList(_transition1Class);

            _levelNameLabel = this.Q("LevelNameLabel");
            _levelStartLable = this.Q("LevelStartLabel");
        }

        public new sealed class UxmlFactory : UxmlFactory<LevelStartView, UxmlTraits> { }
    }
}
