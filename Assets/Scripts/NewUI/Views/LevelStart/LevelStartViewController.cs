using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.UI.Views.LevelStart {
    public class LevelStartViewController : ViewController<LevelStartView> {
        [SerializeField] private ViewController _viewAfterStart;

        public override void ShowView() {
            base.ShowView();
            _ = StartAnimation();
        }

        private async Awaitable StartAnimation() {
            const float showAnimationDelay = 0.5f;
            const float hideAnimationDelay = 0.2f;

            await Awaitable.NextFrameAsync();

            view.ShowLevelName();
            await Awaitable.WaitForSecondsAsync(2f);
            view.HideLevelName();
            await Awaitable.WaitForSecondsAsync(hideAnimationDelay);

            view.ShowThirdCount();
            await Awaitable.WaitForSecondsAsync(showAnimationDelay);
            view.HideThirdCount();
            await Awaitable.WaitForSecondsAsync(hideAnimationDelay);

            view.ShowSecondCount();
            await Awaitable.WaitForSecondsAsync(showAnimationDelay);
            view.HideSecondCount();
            await Awaitable.WaitForSecondsAsync(hideAnimationDelay);

            view.ShowFirstCount();
            await Awaitable.WaitForSecondsAsync(showAnimationDelay);
            view.HideFirstCount();
            await Awaitable.WaitForSecondsAsync(hideAnimationDelay);

            view.ShowStartLabel();
            await Awaitable.WaitForSecondsAsync(showAnimationDelay);
            view.HideStartLabel();

            ScreenController.ShowView(_viewAfterStart);
        }
    }
}
