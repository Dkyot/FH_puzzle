using UnityEngine;
using EasyTransition;

namespace FH.Init {
    public class SceneTransitionManager : MonoBehaviour {
        [Header("Settings")]
        public TransitionSettings transitionSettings;


        public async Awaitable StartTransition() {
            // Start transition
            TransitionManager.Instance().Transition(transitionSettings, 0);

            OnTransitionIn();
            // Wait full in transition time
            await Awaitable.WaitForSecondsAsync(transitionSettings.transitionTime);
            OnTransitionOut();
        }

        protected virtual void OnTransitionIn() { }

        protected virtual void OnTransitionOut() { }
    }
}
