using UnityEngine;
using FH.Sound;

namespace FH.Init {
    public sealed class SceneTransitionManagerWithOneTransitionSound : SceneTransitionManager {
        [Header("Sounds")]
        public AudioClip transitionSound;

        protected sealed override void OnTransitionIn() {
            SoundManager.Instance.PlayOneShot(transitionSound);
        }
    }
}
