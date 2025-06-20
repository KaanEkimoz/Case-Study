using DG.Tweening;
using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] float comboResetTime = 1.2f;
    [SerializeField] float inputWindow = 0.6f;

    [Header("Damage Values")]
    [SerializeField] int softAttackDamage = 10;
    [SerializeField] int heavyAttackDamage = 20;

    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] WeaponHitbox weaponHitboxOnHand;

    [Header("Attack Motion")]
    [SerializeField] float attackForwardDistance = 0.5f;
    [SerializeField] float attackForwardDuration = 0.1f;
    [SerializeField] Ease attackForwardEase = Ease.OutQuad;

    [Header("Obstacle Detect")]
    [SerializeField] float obstacleCheckDistance = 1f;
    [SerializeField] LayerMask obstacleLayer;


    int currentComboStep = 0;
    float lastAttackTime;
    bool canChainNextAttack;
    ThirdPersonController thirdPersonController;

    void Update()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();

        if (PlayerInputHandler.Instance.AttackButtonPressed)
            TryAttack();

        if (Time.time - lastAttackTime > comboResetTime)
        {
            ResetCombo();
            weaponHitboxOnHand.DisableWeaponHitbox();
        }
            
    }

    void TryAttack()
    {
        if (currentComboStep == 0)
            StartAttack();
        else if (canChainNextAttack && currentComboStep < 3)
            ChainAttack();
    }

    void StartAttack()
    {
        currentComboStep = 1;
        lastAttackTime = Time.time;
        canChainNextAttack = false;

        animator.SetTrigger("Attack1");
        Invoke(nameof(EnableNextAttack), inputWindow);
    }

    void ChainAttack()
    {
        currentComboStep++;
        lastAttackTime = Time.time;
        canChainNextAttack = false;

        string trigger = "Attack" + currentComboStep;
        animator.SetTrigger(trigger);

        if (currentComboStep < 3)
            Invoke(nameof(EnableNextAttack), inputWindow);
    }

    void EnableNextAttack()
    {
        canChainNextAttack = true;
    }

    void ResetCombo()
    {
        currentComboStep = 0;
        canChainNextAttack = false;
    }
    
    public void OnAttackAnimationStarted()
    {
        int damageFromAttack = (currentComboStep < 3) ? softAttackDamage : heavyAttackDamage;
        weaponHitboxOnHand.ActivateWeaponHitbox(damageFromAttack,damageFromAttack == heavyAttackDamage);
        SoundFXManager.Instance.PlayRandomSoundFXAtPosition(SoundFXManager.Instance.swordSwingSounds, transform, 0.15f);

        if (!IsObstacleInFront())
        {
            Vector3 forwardDir = transform.forward * attackForwardDistance;
            transform.DOMove(transform.position + forwardDir, attackForwardDuration)
                     .SetEase(attackForwardEase);
        }

        thirdPersonController.movementSpeedMultiplierWhenAttacking = 0.3f;
        thirdPersonController.canRotate = false;
    }
    private bool IsObstacleInFront()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 direction = transform.forward;

        return Physics.Raycast(origin, direction, obstacleCheckDistance, obstacleLayer);
    }
    public void OnAttackAnimationFinished()
    {
        weaponHitboxOnHand.SetTrailMaterialColor(Color.white);
        thirdPersonController.movementSpeedMultiplierWhenAttacking = 1;
        thirdPersonController.canRotate = true;
        // weaponHitboxOnHand.DisableWeaponHitbox();
    }
}
