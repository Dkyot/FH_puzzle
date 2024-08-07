using FH.Sound;
using Platforms.Main;
using Platforms.Metrika;
using UnityEngine;

namespace FH.UI.Views.LevelStart {
    public class LevelStartViewController : ViewController<LevelStartView> {
        [SerializeField] private ViewController _viewAfterStart;

        [Header("Sounds")]
        [SerializeField] private AudioClip _startSound;
        [SerializeField] private AudioClip _contDownSound;
        [SerializeField] private AudioClip _lastCoundDownSound;

        private readonly float _volumeScale = 0.2f;

        public override void ShowView() {
            base.ShowView();
            PlatformFeatures.Metrika.SendEvent(MetrikaEventEnum.LevelLoaded);
        }

        public async Awaitable StartAnimation() {
            const float showAnimationDelay = 0.4f;
            const float hideAnimationDelay = 0.15f;

            var soundManager = SoundManager.Instance;

            await Awaitable.NextFrameAsync();

            view.ShowLevelName();
            soundManager.Play(_startSound, _volumeScale);
            await Awaitable.WaitForSecondsAsync(1.3f);
            view.HideLevelName();
            await Awaitable.WaitForSecondsAsync(hideAnimationDelay);

            view.ShowThirdCount();
            soundManager.Play(_contDownSound, _volumeScale);
            await Awaitable.WaitForSecondsAsync(showAnimationDelay);
            view.HideThirdCount();
            await Awaitable.WaitForSecondsAsync(hideAnimationDelay);

            view.ShowSecondCount();
            soundManager.Play(_contDownSound, _volumeScale);
            await Awaitable.WaitForSecondsAsync(showAnimationDelay);
            view.HideSecondCount();
            await Awaitable.WaitForSecondsAsync(hideAnimationDelay);

            view.ShowFirstCount();
            soundManager.Play(_contDownSound, _volumeScale);
            await Awaitable.WaitForSecondsAsync(showAnimationDelay);
            view.HideFirstCount();
            await Awaitable.WaitForSecondsAsync(hideAnimationDelay);

            view.ShowStartLabel();
            soundManager.Play(_lastCoundDownSound, _volumeScale);
            await Awaitable.WaitForSecondsAsync(showAnimationDelay);
            view.HideStartLabel();

            ScreenController.ShowView(_viewAfterStart);
        }
    }
}
