using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private Timer timerUI;
    [SerializeField] private ScoreCounter scoreCounter;

    [SerializeField] private Button restartButton;
    
    private void Awake() {
        cardManager = cardManager.GetComponent<CardManager>();
        cardManager.OnWin += OnWin;

        scoreCounter = scoreCounter.GetComponent<ScoreCounter>();

        timerUI = timerUI.GetComponent<Timer>();
        restartButton.onClick.AddListener(RestartButton);
    }

    private void Start() {
        RestartButton();
    }

    private void RestartButton() {
        cardManager?.CreateCards();
        timerUI?.StartTimer();
        scoreCounter?.Reset();
    }
    
    private void OnWin(object sender, EventArgs e) {
        if (timerUI == null) return;
        timerUI.StopTimer();
        scoreCounter?.SaveTime(timerUI.timer);

        Debug.Log(scoreCounter.GetScoreJson());
    }
}
