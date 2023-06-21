using UnityEngine;

public class Card : MonoBehaviour
{
    //[SerializeField] private float timeUntilClosing = 2.5f;
    //private float timer;
    
    private bool isPicked;

    private float rotationSpeed = 0.06f;

    public CardState currentState;

    private CardManager manager;
    
    float reference;

    private void Start() {
        currentState = CardState.Closed;
        manager = FindObjectOfType<CardManager>();
        //timer = 0;
    }

    private void Update() {
        if (isPicked && (currentState == CardState.Closed || currentState == CardState.Opening))
            OpenRotation();
        else if (isPicked && (currentState == CardState.Opened || currentState == CardState.Closing))
            CloseRotation();

        // if (isOpen && !isPicked) {
        //     timer += Time.deltaTime;
        //     if (timer >= timeUntilClosing) {
        //         CloseRotation();
        //     }
        // }
    }

    public void Pick() {
        //Debug.Log(gameObject.name);
        if (manager.TryToFlip(this)) {
            isPicked = true;
        }
    }

    #region Rotation methods
    private void OpenRotation() {
        currentState = CardState.Opening;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 180, ref reference, rotationSpeed);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        if (transform.rotation == Quaternion.Euler(0, 180, 0)) {
            isPicked = false;
            currentState = CardState.Opened;
            
            //timer = 0;
        }  
    }

    private void CloseRotation() {
        currentState = CardState.Closing; 
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 0, ref reference, rotationSpeed);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        if (transform.rotation == Quaternion.Euler(0, 0, 0)) {
            isPicked = false;
            currentState = CardState.Closed;
        }   
    }
    #endregion
}
