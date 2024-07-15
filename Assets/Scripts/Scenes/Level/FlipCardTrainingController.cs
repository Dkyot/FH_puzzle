using System;
using System.Collections;
using FH.Cards;
using FH.Inputs;
using FH.UI.Views.GameUI;
using UnityEngine;

namespace FH.Level
{
    public class FlipCardTrainingController: MonoBehaviour
    {
        [SerializeField] private CardManager cardManager;
        [SerializeField] private GameObject markerPrefab;

        private GameObject _marker;
        private Coroutine _coroutine;

        private void Awake()
        {
            _marker = Instantiate(markerPrefab);
            _marker.SetActive(false);
        }

        public async Awaitable EnableTipAsync()
        {
            EnableTip();
            while (_coroutine != null)
            {
                await Awaitable.NextFrameAsync();
            }
        }
        
        private void EnableTip()
        {
            _coroutine = StartCoroutine(ActivateTipCoroutine());
        }

        public void DisableTip()
        {
            if (_coroutine == null) return;
            StopCoroutine(_coroutine);
            _marker.SetActive(false);
            _coroutine = null;
        }

        private IEnumerator ActivateTipCoroutine()
        {
            var card = cardManager.GetFirstCard();
            if (card == null) yield break;
            _marker.SetActive(false);
            _marker.transform.position = card.transform.position;
            _marker.transform.localScale = new Vector3(card.transform.localScale.x, card.transform.localScale.y, 1);
            _marker.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            card.Pick();
            while (card.CurrentState != CardState.Opened)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            card.Pick();
            while (card.CurrentState != CardState.Closed)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            _marker.SetActive(false);
            _coroutine = null;
        }
    }
}