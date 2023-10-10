using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI {
    public sealed class SafeArea : VisualElement {
        public float Margin_Top { get; set; }
        public float Margin_Bottom { get; set; }
        public float Margin_Right { get; set; }
        public float Margin_Left { get; set; }
        public SafeArea() {
            UpdateSafeArea();
            RegisterCallback<GeometryChangedEvent>(LayoutChanged);
        }

        private void UpdateSafeArea() {
            (Vector2 leftTop, Vector2 rightBottom) = GetSafeArea(panel);
            style.marginLeft = Mathf.Max(leftTop.x, Margin_Left);
            style.marginTop = Mathf.Max(leftTop.y, Margin_Top);
            style.marginRight = Mathf.Max(rightBottom.x, Margin_Right);
            style.marginBottom = Mathf.Max(rightBottom.y, Margin_Bottom);
        }

        private void LayoutChanged(GeometryChangedEvent e) {
            UpdateSafeArea();
        }

        private static (Vector2 leftTop, Vector2 rightBottom) GetSafeArea(IPanel panel) {
            if (panel == null)
                return (Vector2.zero, Vector2.zero);

            try {
                var leftTop = RuntimePanelUtils.ScreenToPanel(
                    panel,
                    new Vector2(Screen.safeArea.xMin, Screen.height - Screen.safeArea.yMax)
                );
                var rightBottom = RuntimePanelUtils.ScreenToPanel(
                    panel,
                    new Vector2(Screen.width - Screen.safeArea.xMax, Screen.safeArea.yMin)
                );

                return (leftTop, rightBottom);
            }
            catch {
                return (Vector2.zero, Vector2.zero);
            }
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlFloatAttributeDescription _top = new UxmlFloatAttributeDescription() { name = "Margin_Top", defaultValue = 0 };
            private UxmlFloatAttributeDescription _left = new UxmlFloatAttributeDescription() { name = "Margin_Left", defaultValue = 0 };
            private UxmlFloatAttributeDescription _bottom = new UxmlFloatAttributeDescription() { name = "Margin_Bottom", defaultValue = 0 };
            private UxmlFloatAttributeDescription _right = new UxmlFloatAttributeDescription() { name = "Margin_Right", defaultValue = 0 };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                if (ve is not SafeArea safeArea)
                    return;

                safeArea.Margin_Top = _top.GetValueFromBag(bag, cc);
                safeArea.Margin_Bottom = _bottom.GetValueFromBag(bag, cc);
                safeArea.Margin_Right = _right.GetValueFromBag(bag, cc);
                safeArea.Margin_Left = _left.GetValueFromBag(bag, cc);

                safeArea.UpdateSafeArea();
            }
        }

        public new sealed class UxmlFactory : UxmlFactory<SafeArea, UxmlTraits> {
        }
    }
}
