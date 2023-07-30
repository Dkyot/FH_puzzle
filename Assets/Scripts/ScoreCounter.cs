using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private int mismatchCounter;
    [SerializeField] private float timer;
    [SerializeField] private float finalScore;
    
    private void Awake() {
        AddCardManagerEvents();
        Reset();
    }
    
    private void Mismatch() {
        mismatchCounter++;
    }

    public void Reset() {
        mismatchCounter = 0;
        timer = 0;
    }

    public void SaveTime(float timer) {
        this.timer = timer;
        CalculateScore();
    }

    private void CalculateScore() {
        finalScore = 10000 - (timer * 4) - (mismatchCounter * 17);
        if (finalScore <= 0) finalScore = 1;
    }

    public string GetScoreJson() {
        return JsonUtility.ToJson(this);
    }
    
    #region Subscribe to events
    private void AddCardManagerEvents() {
        CardManager.OnMismatch += Mismatch;
    }
    #endregion
}
