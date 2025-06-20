using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputHandler : MonoBehaviour
{
    #region Singleton
    public static PlayerInputHandler Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    public bool AttackButtonPressed => _attackButtonPressedThisFrame;
    public bool AttackButtonReleased => _attackButtonReleasedThisFrame;
    public bool AttackButtonHeld => _attackButtonHeld;
    public bool SprintButtonHeld => _sprintButtonHeld;
    public bool LockOnButtonPressed => _lockOnButtonPressedThisFrame;
    public bool LockOnButtonReleased => _lockOnButtonReleasedThisFrame;

    public bool LockOnButtonHeld => _lockOnButtonHeld;

    public Vector2 MovementInput => _moveInput;
    public Vector2 MouseInput => _mouseInput;

    // Movement - WASD Keyboard Buttons, Mouse Cursor
    private Vector2 _moveInput;
    private Vector2 _mouseInput;

    // Sprint - Left Shift
    private bool _sprintButtonHeld;

    // Attack - Left Mouse Button
    private bool _attackButtonPressedThisFrame;
    private bool _attackButtonReleasedThisFrame;
    private bool _attackButtonHeld;

    // Lock On Target - TAB Keyboard Button
    private bool _lockOnButtonPressedThisFrame;
    private bool _lockOnButtonReleasedThisFrame;
    private bool _lockOnButtonHeld;

    [Header("Movement Settings")]
    public bool analogMovement;

    private void LateUpdate()
    {
        ResetInputFlags();
    }
    private void ResetInputFlags()
    {
        _attackButtonPressedThisFrame = false;
        _attackButtonReleasedThisFrame = false;
        _lockOnButtonPressedThisFrame = false;
        _lockOnButtonReleasedThisFrame = false;
    }

    #region Mouse Cursor

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }
    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
    public Vector3 GetMouseWorldPosition(LayerMask cursorDetectMask)
    {
        if (Camera.main == null)
            return Vector3.zero;

        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, cursorDetectMask))
            return hitInfo.point;

        return Vector3.zero;
    }

    #endregion

    #region Input Functions

    public void OnMovement(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseInput = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started)
            _sprintButtonHeld = true;
        else if (context.canceled)
            _sprintButtonHeld = false;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _attackButtonHeld = true;
            _attackButtonPressedThisFrame = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _attackButtonHeld = false;
            _attackButtonReleasedThisFrame = true;
        }
    }
    public void OnLockOn(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _lockOnButtonHeld = true;
            _lockOnButtonPressedThisFrame = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _lockOnButtonHeld = false;
            _lockOnButtonReleasedThisFrame = true;
        }
    }
    #endregion
}
