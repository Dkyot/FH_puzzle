using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Controls input;
    
    private void Awake() {
        input = new Controls();
    }
    
    private void OnClick(InputAction.CallbackContext context) {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider == null) return;
        
        Interaction(hit);
    }

    private void Interaction(RaycastHit2D hit) {
        Card card = hit.collider.gameObject.GetComponent<Card>();
        if (card != null)
            card.Pick();
    }

    #region InputSystem subscriptions
    private void OnEnable() {
        input.Enable();
        input.InGame.LMB_click.performed += OnClick;
    }

    private void OnDisable() {
        input.Disable();
        input.InGame.LMB_click.performed -= OnClick;
    }
    #endregion
}
