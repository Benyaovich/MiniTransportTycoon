using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnMovePerformed;
    public event EventHandler OnLeftClickPressed;
    public event EventHandler OnScrollPerformed;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += MoveOnPerformed;
        playerInputActions.Player.LeftClick.performed += LeftClickOnPerformed;
        playerInputActions.Player.Scroll.performed += ScrollOnPerformed;
        
    }

    

    private void OnDisable()
    {
        playerInputActions.Player.Move.performed -= MoveOnPerformed;
        playerInputActions.Player.LeftClick.performed -= LeftClickOnPerformed;
        playerInputActions.Player.Scroll.performed -= ScrollOnPerformed;
        playerInputActions.Dispose();
    }
    
    private void ScrollOnPerformed(InputAction.CallbackContext obj)
    {
        OnScrollPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void LeftClickOnPerformed(InputAction.CallbackContext obj)
    {
        OnLeftClickPressed?.Invoke(this, EventArgs.Empty);
    }
    
    private void MoveOnPerformed(InputAction.CallbackContext obj)
    {
        OnMovePerformed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public float GetScrollAxis() => playerInputActions.Player.Scroll.ReadValue<float>();
}
