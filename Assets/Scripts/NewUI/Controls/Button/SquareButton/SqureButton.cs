using UnityEngine.UIElements;

namespace FH.UI {
    public class SquareButton : TextButtonBase {
        public const string squareButtonClassName = "square_button";

        protected override string BaseStyleName => squareButtonClassName;

        public new sealed class UxmlFactory : UxmlFactory<SquareButton, UxmlTraits> { }
    }
}