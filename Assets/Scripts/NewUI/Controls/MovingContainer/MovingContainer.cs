using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI {
    public sealed class MovingContainer : VisualElement {
        public const string movingContainerBaseStyle = "moving-container";
        public const string movingContainerTransitionStyle = "moving-container-transition";

        public MovingContainer() {
            AddToClassList(movingContainerBaseStyle);
            AddToClassList(movingContainerTransitionStyle);

            RegisterCallback<TransitionEndEvent>(
                (evnt) => {
                    ToggleInClassList(movingContainerTransitionStyle);
                });

            // Баг с низким значение задержки

            // Если значение недостаточно большое,
            // то при старте вызов коллбэка произойдёт,
            // но анимация не начнёт проигрываться.

            // Нужно протестить в билде. 

            // Необходимо, что бы запустить анимацию 
            schedule.Execute(
                () => {
                    ToggleInClassList(movingContainerTransitionStyle);
                }
            ).StartingIn(1000);
        }

        public new sealed class UxmlFactory : UxmlFactory<MovingContainer, UxmlTraits> { }
    }
}