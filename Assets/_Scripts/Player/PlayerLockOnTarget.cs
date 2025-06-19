using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ThirdPersonController))]
public class PlayerLockOnTarget : MonoBehaviour
{

    [Header("References")]
    [SerializeField] LayerMask targetLayers;
    [SerializeField] Transform lockOnCanvas;
    [SerializeField] Transform enemyTargetLocator;
    //[SerializeField] Animator cinemachineAnimator;
    [SerializeField] GameObject lockOnCamera;

    [Header("Settings")]
    [SerializeField] bool flattenVerticalLook = true;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float smoothing = 2f;
    [SerializeField, Tooltip("Max detection angle in degrees")] float maxDetectionAngle = 60f;
    [SerializeField] float crosshairScaleMultiplier = 0.1f;

    private Transform lockOnTarget;
    Animator animator;
    ThirdPersonController thirdPersonController;
    Transform mainCamera;
    float currentYOffset;
    bool isLockedOn;
    Vector3 cachedTargetPosition;

    void Start()
    {
        if(animator == null)
            animator = GetComponent<Animator>();

        if(thirdPersonController == null)
            thirdPersonController = GetComponent<ThirdPersonController>();

        if(mainCamera == null)
            mainCamera = Camera.main.transform;

        if(lockOnCanvas != null)
            lockOnCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        //cameraFollow.lockedTarget = isLockedOn;
        thirdPersonController.lockMovement = isLockedOn;

        if (PlayerInputHandler.Instance.LockOnButtonPressed)
            ToggleLockOn();

        if (isLockedOn)
        {
            if (!IsTargetInRange())
            {
                ClearTarget();
                return;
            }
            RotateTowardsTarget();
        }
    }

    void ToggleLockOn()
    {
        if (lockOnTarget != null)
        {
            ClearTarget();
            return;
        }

        lockOnTarget = FindNearestTarget();

        if (lockOnTarget != null)
            LockOnToTarget();
    }

    void LockOnToTarget()
    {
        isLockedOn = true;
        animator.SetLayerWeight(1, 1);
        lockOnCamera.SetActive(true);
        lockOnCamera.GetComponent<CinemachineCamera>().LookAt = lockOnTarget;
        lockOnCanvas.gameObject.SetActive(true);
    }

    void ClearTarget()
    {
        isLockedOn = false;
        lockOnTarget = null;
        enemyTargetLocator.transform.position = transform.position;
        animator.SetLayerWeight(1, 0);
        lockOnCamera.GetComponent<CinemachineCamera>().LookAt = null;
        lockOnCamera.SetActive(false);
        lockOnCanvas.gameObject.SetActive(false);
    }

    Transform FindNearestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetLayers);
        if (hits.Length == 0) return null;

        float bestAngle = maxDetectionAngle;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = hit.transform.position - mainCamera.position;
            directionToTarget.y = 0;
            float angle = Vector3.Angle(mainCamera.forward, directionToTarget);

            if (angle < bestAngle && !IsObstructed(hit.transform))
            {
                bestTarget = hit.transform;
                bestAngle = angle;
            }
        }

        if (bestTarget == null) return null;

        CalculateYOffset(bestTarget);
        return bestTarget;
    }

    void CalculateYOffset(Transform target)
    {
        CapsuleCollider col = target.GetComponent<CapsuleCollider>();
        float trueHeight = col.height * target.localScale.y;
        currentYOffset = trueHeight * 0.75f;

        if (flattenVerticalLook && currentYOffset > 1.6f && currentYOffset < 1.6f * 3f)
            currentYOffset = 1.6f;
    }

    bool IsObstructed(Transform target)
    {
        Vector3 eyePosition = transform.position + Vector3.up * 0.5f;
        Vector3 targetPosition = target.position + Vector3.up * currentYOffset;

        if (Physics.Linecast(eyePosition, targetPosition, out RaycastHit hit))
        {
            return !hit.transform.CompareTag("Enemy");
        }

        return false;
    }

    bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, cachedTargetPosition);
        return distance / 2f <= detectionRadius;
    }

    void RotateTowardsTarget()
    {
        if (lockOnTarget == null)
        {
            ClearTarget();
            return;
        }

        cachedTargetPosition = lockOnTarget.position + Vector3.up * currentYOffset;

        lockOnCanvas.position = cachedTargetPosition;
        lockOnCanvas.localScale = Vector3.one * Vector3.Distance(mainCamera.position, cachedTargetPosition) * crosshairScaleMultiplier;

        enemyTargetLocator.position = cachedTargetPosition;

        Vector3 lookDirection = lockOnTarget.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothing);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
