using NUnit.Framework;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace FH.UI {
    [UxmlElement]
    public sealed partial class LoadingDots : Label {
        private const string _dotsFirstStep = ".";
        private const string _dotsSecodStep = "..";
        private const string _dotsThirdStep = "...";
        private int _currentStep = 0;

        private CancellationTokenSource _tokenSource;

        public LoadingDots() {
            text = _dotsFirstStep;

            if (Application.isPlaying) {
                RegisterCallback<AttachToPanelEvent>((e) => StartAnimation());
                RegisterCallback<DetachFromPanelEvent>((e) => CancelAnimation());
            }
        }

        public void StartAnimation() {
            if (_tokenSource != null)
                return;
            _tokenSource = new CancellationTokenSource();
            _ = Animate(_tokenSource.Token);
        }

        public void CancelAnimation() {
            _tokenSource?.Cancel();
            _tokenSource = null;
        }


        private async Awaitable Animate(CancellationToken cancellationToken) {
            while (!cancellationToken.IsCancellationRequested) {
                await Awaitable.WaitForSecondsAsync(0.5f, cancellationToken);

                _currentStep++;
                var stepIndex = _currentStep % 3;

                text = stepIndex switch {
                    0 => _dotsFirstStep,
                    1 => _dotsSecodStep,
                    2 => _dotsThirdStep,
                    _ => _dotsFirstStep,
                };
            }
        }
    }
}
