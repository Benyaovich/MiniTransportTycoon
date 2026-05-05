using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public bool IsRightClickPressed { get; private set; } = false;

    public event EventHandler OnMovePerformed;
    public event EventHandler OnLeftClickPressed;
    public event EventHandler OnDeleteKeyPressed;

    public event EventHandler OnLeftClickStarted;
    public event EventHandler OnRightClickPressed;
    public event EventHandler OnLeftClickCanceled;
    
    private PlayerInputActions _playerInputActions;


    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Move.performed += MoveOnPerformed;
        
        _playerInputActions.Player.LeftClick.performed += LeftClickOnPerformed;
        _playerInputActions.Player.LeftClick.started += LeftClickOnStarted;
        _playerInputActions.Player.LeftClick.canceled += LeftClickOnCanceled;
        
        _playerInputActions.Player.RightClick.performed += RightClickOnPerformed;
        _playerInputActions.Player.RightClick.started += RightClickOnStarted;
        _playerInputActions.Player.RightClick.canceled += RightClickOnCanceled;
        
        
        _playerInputActions.Player.Delete.performed += DeleteOnPerformed;

    }

    


    private void OnDisable()
    {
        _playerInputActions.Player.Move.performed -= MoveOnPerformed;
        
        _playerInputActions.Player.LeftClick.canceled -= LeftClickOnCanceled;
        _playerInputActions.Player.LeftClick.started -= LeftClickOnStarted;
        _playerInputActions.Player.LeftClick.performed -= LeftClickOnPerformed;
        
        _playerInputActions.Player.RightClick.performed -= RightClickOnPerformed;
        _playerInputActions.Player.RightClick.started -= RightClickOnStarted;
        _playerInputActions.Player.RightClick.canceled -= RightClickOnCanceled;
        
        _playerInputActions.Player.Delete.performed -= DeleteOnPerformed;
        
        _playerInputActions.Player.Disable();
        _playerInputActions.Dispose();
    }

    private void DeleteOnPerformed(InputAction.CallbackContext obj)
    {
        OnDeleteKeyPressed?.Invoke(this, EventArgs.Empty);
    }
    private void LeftClickOnCanceled(InputAction.CallbackContext obj)
    {
        OnLeftClickCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void LeftClickOnStarted(InputAction.CallbackContext obj)
    {
        OnLeftClickStarted?.Invoke(this, EventArgs.Empty);
    }
    
    private void RightClickOnCanceled(InputAction.CallbackContext obj)
    {
        IsRightClickPressed = false;
    }

    private void RightClickOnStarted(InputAction.CallbackContext obj)
    {
        IsRightClickPressed = true;
    }
    
    private void RightClickOnPerformed(InputAction.CallbackContext obj)
    {
        OnRightClickPressed?.Invoke(this, EventArgs.Empty);
    }


    public Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
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
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public float GetScrollAxis() => _playerInputActions.Player.Scroll.ReadValue<float>();
}
