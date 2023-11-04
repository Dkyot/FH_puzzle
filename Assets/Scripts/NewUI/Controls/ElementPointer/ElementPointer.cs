using UnityEngine.UIElements;

namespace FH.UI
{
    public class ElementPointer : VisualElement
    {
        private const string elementPointerClassName = "element-pointer";
        private const string elementPointerAnimationClassName = "element-pointer-animation";
        
        public void StartAnimation()
        {
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            RegisterCallback<TransitionEndEvent>(OnTransitionEnded);
            schedule.Execute(() => ToggleInClassList(elementPointerAnimationClassName)).StartingIn(100);
        }

        public void StopAnimation()
        {
            RemoveFromClassList(elementPointerAnimationClassName);
            UnregisterCallback<TransitionEndEvent>(OnTransitionEnded);
            style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }

        private void OnTransitionEnded(TransitionEndEvent evt)
        {
            ToggleInClassList(elementPointerAnimationClassName);
        }

        public ElementPointer()
        {
            AddToClassList(elementPointerClassName);
        }
        
        public new sealed class UxmlFactory : UxmlFactory<ElementPointer, UxmlTraits> { }
    }
}