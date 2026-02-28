using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnLeftClickPressed;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.LeftClick.performed += LeftClickOnPerformed;
        
    }


    private void OnDestroy()
    {
        playerInputActions.Player.LeftClick.performed -= LeftClickOnPerformed;
        playerInputActions.Dispose();
    }
    private void LeftClickOnPerformed(InputAction.CallbackContext obj)
    {
        OnLeftClickPressed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }
}
