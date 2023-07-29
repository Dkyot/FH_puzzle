using UnityEngine.UIElements;

namespace FH.UI {
    public sealed class SquareIconButton : IconButtonBase {
        public const string squareIconButtonStyle = "square_icon_button";
        
        protected override string BaseStyleName => squareIconButtonStyle;

        public new sealed class UxmlFactory : UxmlFactory<SquareIconButton, UxmlTraits> { }
    }
}