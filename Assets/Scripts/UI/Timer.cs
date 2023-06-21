using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float timer;
    private bool isRun;

    private void Awake() {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        StartTimer();
    }

    private void Update() {
        if (!isRun) return;

        timer += Time.deltaTime;
        string minutes = ((int) timer / 60).ToString();
        string seconds = (timer % 60).ToString("f1");
        timerText.text = minutes + ':' + seconds;
    }

    public void StartTimer() {
        timer = 0;
        isRun = true;
    }

    public void StopTimer() {
        isRun = false;
    }
}
