using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Animator))]

public class ThirdPersonController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float gravity = 25f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float speedSmoothTime = 0.1f;
    [Space]
    [Header("References")]
    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;

    private float _velocityY;
    private float _currentSpeed;
    private float _speedSmoothVelocity;

    private Vector2 inputMove;
    Vector3 moveDirection;

    public bool lockMovement;

    void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if(animator == null)
            animator = GetComponent<Animator>();

        if (mainCameraTransform == null && Camera.main != null)
            mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        ReadInput();
        CalculateMovement();

        if (!lockMovement)
            ApplyRotation();

        ApplyGravityAndMove();
        UpdateAnimator();
    }

    void ReadInput()
    {
        inputMove.x = Input.GetAxis("Horizontal");
        inputMove.y = Input.GetAxis("Vertical");

        moveDirection = GetCameraRelativeDirection(inputMove).normalized;
    }

    Vector3 GetCameraRelativeDirection(Vector2 input)
    {
        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        return forward.normalized * input.y + right.normalized * input.x;
    }
    void CalculateMovement()
    {
        float targetSpeed = moveDirection.magnitude > 0 ? moveSpeed : 0;
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
        animator.SetFloat("Movement", moveDirection.magnitude, 0.1f, Time.deltaTime);
        animator.SetFloat("Horizontal", inputMove.x, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", inputMove.y, 0.1f, Time.deltaTime);
    }
}
