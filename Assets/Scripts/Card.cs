using UnityEngine;

public class Card : MonoBehaviour
{
    private bool isPicked;
    private bool isOpen;

    private float rotationSpeed = 0.06f;
    
    float reference;

    private void Update() {
        if (isPicked && !isOpen)
            OpenRotation();
        else if (isPicked && isOpen)
            CloseRotation();
    }

    public void Pick() {
        Debug.Log(gameObject.name);
        isPicked = true;
    }

    #region Rotation methods
    private void OpenRotation() {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 180, ref reference, rotationSpeed);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        if (transform.rotation == Quaternion.Euler(0, 180, 0)) {
            isPicked = false;
            isOpen = true;
        }  
    }

    private void CloseRotation() {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 0, ref reference, rotationSpeed);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        if (transform.rotation == Quaternion.Euler(0, 0, 0)) {
            isPicked = false;
            isOpen = false;
        }   
    }
    #endregion
}
