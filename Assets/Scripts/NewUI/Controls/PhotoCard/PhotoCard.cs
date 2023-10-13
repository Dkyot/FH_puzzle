using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI {
    public sealed class PhotoCard : VisualElement {
        public const string photoCardClass = "photo-card";
        public const string photoCardImageClass = "photo-card-image";
        public const string hiddenPhotoCardLabelClass = "photo-card-hidden-label";
        public const string photoCardLabelContainerClass = "photo-card-label-container";

        public const string photoCardTextClass = "photo-card-text";
        public const string photoCardHeart = "photo-card-heart";

        public const string photoCardAnimationClass = "photo-card-animation";
        public const string photoCardTransitionClass = "photo-card-transition";

        public string Text {
            get => _text;
            set {
                _text = value;
                SetText();
            }
        }

        public bool IsHidden {
            get => _isHidden;
            set {
                _isHidden = value;
                SetHidden();
            }
        }

        private string _text;
        private bool _isHidden;

        private Label _heartLabel;
        private Label _hiddenPhotoLabel;
        private LocalizedLabel _photoLabel;

        private VisualElement _photoImage;

        public PhotoCard() {
            AddToClassList(photoCardClass);

            _photoImage = new VisualElement();
            _photoImage.AddToClassList(photoCardImageClass);
            Add(_photoImage);

            _hiddenPhotoLabel = new Label {
                text = "?"
            };
            _hiddenPhotoLabel.AddToClassList(hiddenPhotoCardLabelClass);
            _photoImage.Add(_hiddenPhotoLabel);

            var labelContainer = new VisualElement();
            labelContainer.AddToClassList(photoCardLabelContainerClass);
            Add(labelContainer);

            _photoLabel = new LocalizedLabel();
            _photoLabel.ClearClassList();
            _photoLabel.AddToClassList(photoCardTextClass);
            labelContainer.Add(_photoLabel);

            _heartLabel = new Label {
                text = "<3"
            };
            _heartLabel.ClearClassList();
            _heartLabel.AddToClassList(photoCardTextClass);
            _heartLabel.AddToClassList(photoCardHeart);
            labelContainer.Add(_heartLabel);

            SetText();
            SetHidden();
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

        private void SetText() {
            if (_isHidden)
                return;
            _photoLabel.Label = _text;
        }

        private void SetHidden() {
            if (_isHidden) {
                _heartLabel.style.display = DisplayStyle.None;
                _hiddenPhotoLabel.style.display = DisplayStyle.Flex;
                _photoLabel.IsLocalizable = false;
                _photoLabel.Label = "???";
            }
            else {
                _heartLabel.style.display = DisplayStyle.Flex;
                _hiddenPhotoLabel.style.display = DisplayStyle.None;
                _photoLabel.IsLocalizable = true;
                _photoLabel.Label = _text;
            }
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits {
            private UxmlStringAttributeDescription _labelValue = new() { name = "Text", defaultValue = "Sex" };
            private UxmlBoolAttributeDescription _isHiddenValue = new UxmlBoolAttributeDescription { name = "IsHidden", defaultValue = false };

            public UxmlTraits() { }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);

                var ate = ve as PhotoCard;

                var text = _labelValue.GetValueFromBag(bag, cc);
                ate.Text = text;

                bool isHidden = _isHiddenValue.GetValueFromBag(bag, cc);
                ate.IsHidden = isHidden;
            }
        }

        public new sealed class UxmlFactory : UxmlFactory<PhotoCard, UxmlTraits> { }
    }
}