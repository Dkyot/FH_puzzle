﻿using UnityEngine.UIElements;

namespace FH.UI {
    public interface IScreenController {
        UIDocument Document { get; }

        void ShowView(ViewController controller);
    }
}