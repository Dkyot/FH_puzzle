using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI {
    public sealed class SimpleSlider : Slider {
        public const string draggerBackgroundName = "simple-dragger-background";
        public const string leftFillerName = "left-filler";

        private readonly VisualElement _draggerBackground;
        private readonly VisualElement _leftFiller;
        private readonly VisualElement _dragContainer;

        public SimpleSlider() {
            var dragger = this.Q<VisualElement>("unity-dragger");

            _draggerBackground = new VisualElement() {
                name = draggerBackgroundName
            };

            _draggerBackground.AddToClassList(draggerBackgroundName);
            dragger.Add(_draggerBackground);

            _leftFiller = new VisualElement() {
                name = leftFillerName
            };
            _leftFiller.AddToClassList(leftFillerName);

            _dragContainer = this.Q<VisualElement>("unity-drag-container");
            _dragContainer.Insert(0, _leftFiller);
            UpdateFillers(value);
            this.RegisterValueChangedCallback(OnValueChanged);
        }

        private void OnValueChanged(ChangeEvent<float> newValue) {
            UpdateFillers(newValue.newValue);
        }

        private void UpdateFillers(float newValue) {
            float fill = (newValue - lowValue) / (highValue - lowValue);
            var style = _leftFiller.style;

            if (direction == SliderDirection.Horizontal) {
                style.width = _dragContainer.contentRect.width * fill;
            }
            else {
                style.height = _dragContainer.contentRect.height - _dragContainer.contentRect.height * fill;
            }
        }

        private void OnDirectionChanged() {
            var style = _leftFiller.style;
            if (direction == SliderDirection.Vertical) {
                style.width = new StyleLength(StyleKeyword.Null);
            }
            else {
                style.height = new StyleLength(StyleKeyword.Null); 
            }

            UpdateFillers(value);
        }

        public new sealed class UxmlFactory : UxmlFactory<SimpleSlider, UxmlTraits> { }
        public new sealed class UxmlTraits : Slider.UxmlTraits {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                var ate = ve as SimpleSlider;
                ate.OnDirectionChanged();
                ate.UpdateFillers(ate.value);
            }
        }
    }
}