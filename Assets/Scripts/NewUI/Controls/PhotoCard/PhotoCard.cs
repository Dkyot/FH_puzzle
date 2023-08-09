using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI {
    public sealed class PhotoCard : VisualElement {
        public const string photoCardClass = "photo-card";
        public const string photoCardImageClass = "photo-card-image";
        public const string photoCardLabelContainerClass = "photo-card-label-container";

        public const string photoCardTextClass = "photo-card-text";
        public const string photoCardHeart = "photo-card-heart";

        public const string photoCardAnimationClass = "photo-card-animation";
        public const string photoCardTransitionClass = "photo-card-transition";

        public string Text {
            get => _photoLabel.text;
            set => _photoLabel.text = value;
        }

        private Label _photoLabel;
        private VisualElement _photoImage;

        public PhotoCard() {
            AddToClassList(photoCardClass);

            _photoImage = new VisualElement();
            _photoImage.AddToClassList(photoCardImageClass);
            Add(_photoImage);

            var labelContainer = new VisualElement();
            labelContainer.AddToClassList(photoCardLabelContainerClass);
            Add(labelContainer);

            _photoLabel = new Label();
            _photoLabel.ClearClassList();
            _photoLabel.AddToClassList(photoCardTextClass);
            labelContainer.Add(_photoLabel);

            var heartLabel = new Label();
            heartLabel.ClearClassList();
            heartLabel.AddToClassList(photoCardTextClass);
            heartLabel.AddToClassList(photoCardHeart);
            heartLabel.text = "<3";
            labelContainer.Add(heartLabel);
        }

        public void SetImage(Sprite sprite) =>
            _photoImage.style.backgroundImage = new StyleBackground(sprite);

        public void SetImage(Texture2D texture) =>
            _photoImage.style.backgroundImage = new StyleBackground(texture);

        public void StartAnimation() {
            RemoveFromClassList(photoCardTransitionClass);
            AddToClassList(photoCardAnimationClass);
        }

        public void ResetAnimation() {
            RemoveFromClassList(photoCardAnimationClass);
            AddToClassList(photoCardTransitionClass);
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlStringAttributeDescription _labelValue = new() { name = "Text", defaultValue = "Sex" };

            public UxmlTraits() { }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var ate = ve as PhotoCard;
                var text = _labelValue.GetValueFromBag(bag, cc);
                ate.Text = text;
            }
        }

        public new sealed class UxmlFactory : UxmlFactory<PhotoCard, UxmlTraits> { }
    }
}