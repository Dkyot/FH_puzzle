using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public int value { get; private set; }
    private TextMeshProUGUI cardTextValue;
    
    private bool isPicked;
    public bool isMatched;

    private float rotationSpeed = 0.06f;

    public CardState currentState { get; private set; }

    private CardManager manager;

    private float timeUntilClosing = 1.2f;
    private float timer;
    private bool delay;
    
    float reference;

    private void Awake() {
        cardTextValue = GetComponentInChildren<TextMeshProUGUI>();
        manager = FindObjectOfType<CardManager>();
    }

    private void Start() {
        currentState = CardState.Closed;
    }

    private void Update() {
        if (isPicked && (currentState == CardState.Closed || currentState == CardState.Opening))
            OpenRotation();
        else if (isPicked && (currentState == CardState.Opened || currentState == CardState.Closing))
            CloseRotation();

        if (delay) {
            timer += Time.deltaTime;
            if (timer >= timeUntilClosing) {
                CloseRotation();
            }
        }
    }

    public void Pick() {
        //Debug.Log(gameObject.name);
        if (delay) return;
        if (manager.TryToFlip(this)) {
            isPicked = true;
        }
    }

    public void SetValue(int value) {
        this.value = value;
        cardTextValue.text = this.value.ToString();
    }

    public void StartMismatchTimer() {
        delay = true;
    }

    #region Rotation methods
    private void OpenRotation() {
        currentState = CardState.Opening;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 180, ref reference, rotationSpeed);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        if (transform.rotation == Quaternion.Euler(0, 180, 0)) {
            isPicked = false;
            currentState = CardState.Opened;
        }  
    }

    private void CloseRotation() {
        currentState = CardState.Closing; 
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 0, ref reference, rotationSpeed);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        if (transform.rotation == Quaternion.Euler(0, 0, 0)) {
            isPicked = false;
            currentState = CardState.Closed;

            if (delay) {
                delay = false;
                timer = 0;
                manager.Reset();
            }
        }   
    }
    #endregion
}
