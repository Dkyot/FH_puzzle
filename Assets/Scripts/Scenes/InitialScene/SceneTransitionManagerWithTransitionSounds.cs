using UnityEngine;
using FH.Sound;

namespace FH.Init {
    public sealed class SceneTransitionManagerWithTransitionSounds : SceneTransitionManager {
        [Header("Sounds")]
        public AudioClip transitionSound;

        protected override void OnTransitionIn() {
            SoundManager.Instance.PlayOneShot(transitionSound);
        }

        protected override void OnTransitionOut() {
            SoundManager.Instance.PlayOneShot(transitionSound);
        }
    }
}
