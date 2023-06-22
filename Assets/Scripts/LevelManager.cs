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
        //Debug.Log("dfsdf");
        restartButton.onClick.AddListener(RestartButton);
    }

    private void Start() {
        cardManager.CreateCards();
        timerUI.StartTimer();
    }

    private void RestartButton() {
        cardManager.CreateCards();
        timerUI.StartTimer();
    }
    
    private void OnWin(object sender, EventArgs e) {
        timerUI.StopTimer();
    }
}
