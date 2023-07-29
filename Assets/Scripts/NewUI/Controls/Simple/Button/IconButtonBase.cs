using UnityEngine.UIElements;

namespace FH.UI {
    public abstract class IconButtonBase : SimpleButtonBase {
        public const string iconName = "Image";
        public const string iconClassName = "icon";

        protected VisualElement icon;

        public new class UxmlTraits : SimpleButtonBase.UxmlTraits {
            public UxmlTraits() { }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var ate = ve as IconButtonBase;

                var icon = new VisualElement() {
                    name = iconName
                };

                icon.AddToClassList(iconClassName);
                ate.icon = icon;
                ate.background.Add(icon);
            }
        }
    }
}