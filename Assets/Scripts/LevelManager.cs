using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private Timer timerUI;

    [SerializeField] private Button restartButton;
    
    private void Awake() {
        cardManager = cardManager.GetComponent<CardManager>();
        cardManager.OnWin += OnWin;
        timerUI = timerUI.GetComponent<Timer>();
        restartButton.onClick.AddListener(RestartButton);
    }

    private void Start() {
        if (cardManager != null)
            cardManager.CreateCards();
        if (timerUI != null)
            timerUI.StartTimer();
    }

    private void RestartButton() {
        if (cardManager != null)
            cardManager.CreateCards();
        if (timerUI != null)
            timerUI.StartTimer();
    }
    
    private void OnWin(object sender, EventArgs e) {
        if (timerUI != null)
            timerUI.StopTimer();
    }
}
