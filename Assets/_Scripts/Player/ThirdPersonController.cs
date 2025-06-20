using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]

public class ThirdPersonController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float gravity = 25f;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float speedSmoothTime = 0.15f;
    [Space]
    [Header("References")]
    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private float movementDampingTime = 0.1f;

    private float _velocityY;
    private float _currentSpeed;
    private float _speedSmoothVelocity;
    public float movementSpeedMultiplierWhenAttacking = 1;

    private Vector3 moveDirection;

    public bool lockMovement;
    public bool canRotate;

    public bool canMove = true;

    void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if(animator == null)
            animator = GetComponent<Animator>();

        if (mainCameraTransform == null && Camera.main != null)
            mainCameraTransform = Camera.main.transform;

        canRotate = true;
        movementSpeedMultiplierWhenAttacking = 1;
    }

    private void Update()
    {
        ReadInput();
        CalculateMovement();

        if (!lockMovement && canRotate)
            ApplyRotation();

        ApplyGravityAndMove();


        UpdateAnimator();
    }

    private void ReadInput()
    {
        moveDirection = GetCameraRelativeDirection(PlayerInputHandler.Instance.MovementInput);
    }

    Vector3 GetCameraRelativeDirection(Vector2 movementInput)
    {
        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        return forward.normalized * movementInput.y + right.normalized * movementInput.x;
    }
    void CalculateMovement()
    {
        float targetSpeed = moveDirection.magnitude > 0 ? moveSpeed * movementSpeedMultiplierWhenAttacking : 0;
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, speedSmoothTime);
    }
    void ApplyGravityAndMove()
    {
        if (controller.isGrounded && _velocityY < 0)
            _velocityY = -2f;

        _velocityY -= gravity * Time.deltaTime;

        Vector3 velocity = moveDirection * _currentSpeed + Vector3.up * _velocityY;
        controller.Move(velocity * Time.deltaTime);
    }
    void ApplyRotation()
    {
        if (moveDirection.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void UpdateAnimator()
    {
        animator.SetFloat("Movement", moveDirection.magnitude, movementDampingTime, Time.deltaTime);
        animator.SetFloat("XAxis", moveDirection.x, movementDampingTime, Time.deltaTime);
        animator.SetFloat("ZAxis", moveDirection.z, movementDampingTime, Time.deltaTime);
    }
    public void OnStepAnimationEvent()
    {
        SoundFXManager.Instance.PlayRandomSoundFXAtPosition(SoundFXManager.Instance.playerWalkOnConcreteSounds, transform, 0.15f);
    }
}
