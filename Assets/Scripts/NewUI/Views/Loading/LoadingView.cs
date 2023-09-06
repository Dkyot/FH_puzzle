using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI.Views.Loading {
    public class LoadingView : ViewBase {
        protected override void OnInit() {
        }

        public new sealed class UxmlFactory : UxmlFactory<LoadingView, UxmlTraits> { }
    }
}
