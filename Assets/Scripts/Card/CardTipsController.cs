using System;
using UnityEngine;
using UnityEngine.Events;

public class CardTipsController : MonoBehaviour
{
    private float timer = 0;
    private float tipTimer = 5;
    private bool tipIsActive = false;
    
    public delegate void OnActivateDelegate();
    public static event OnActivateDelegate OnActivate;

    private void Awake() {
        AddCardManagerEvents();
    }

    private void Update() {
        if (tipIsActive) return;
        
        timer += Time.deltaTime;
        if (timer >= tipTimer) {
            OnActivate();
            tipIsActive = true;
        }
    }

    private void ResetTheTipTimer() {
        timer = 0;
        tipIsActive = false;
    }

    private void AddCardManagerEvents() {
        CardManager.OnMatchCheck += ResetTheTipTimer;
        CardManager.OnReset += ResetTheTipTimer;
    }
}
