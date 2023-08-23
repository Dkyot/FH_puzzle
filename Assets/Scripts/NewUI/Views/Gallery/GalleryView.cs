using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.Gallery {
    public class GalleryView : ViewBase {
        public override void Init() {
        }

        public new sealed class UxmlFactory : UxmlFactory<GalleryView, UxmlTraits> { }
    }
}
