using System;

namespace FH.UI.MainMenu {
    public interface IMainMenu {
        event Action playPressed;
        event Action galleryPressed;
        event Action settingsPressed;

        void Init();
        void Activate();
        void Disable();
    }
}