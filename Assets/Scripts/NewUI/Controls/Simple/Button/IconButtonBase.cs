using UnityEngine.UIElements;

namespace FH.UI {
    public abstract class IconButtonBase : SimpleButtonBase {
        public const string iconName = "Image";
        public const string iconClassName = "icon";

        protected VisualElement icon;
        
        public IconButtonBase() : base() {
            var icon = new VisualElement() {
                name = iconName
            };

            icon.AddToClassList(iconClassName);
            background.Add(icon); 
        }
    }
}