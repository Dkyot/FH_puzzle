using System.Collections;
using FH.Cards;
using FH.Inputs;
using FH.UI.Views.GameUI;
using UnityEngine;

namespace FH.Level
{
    public class FlipCardTipController: MonoBehaviour
    {
        [SerializeField] private CardManager cardManager;

        public async Awaitable ActivateTip()
        {
            var card = cardManager.GetFirstCard();
            if (card == null) return;
            card.Pick();
            while (card.CurrentState != CardState.Opened)
            {
                await Awaitable.NextFrameAsync();
            }

            await Awaitable.WaitForSecondsAsync(0.5f);
            card.Pick();
            while (card.CurrentState != CardState.Closed)
            {
                await Awaitable.NextFrameAsync();
            }
        }

        private IEnumerator Coroutine()
        {
            var card = cardManager.GetFirstCard();
            if (card == null) yield break;
            card.StartFullRotation();
            // card.Pick();
            // yield return new WaitForSeconds(0.5f);
            // if (card.CurrentState == CardState.Opened)
            // {
            //     card.Pick();
            // }
        }
    }
}