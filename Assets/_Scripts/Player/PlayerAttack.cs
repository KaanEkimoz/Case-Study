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

    int currentComboStep = 0;
    float lastAttackTime;
    bool canChainNextAttack;

    void Update()
    {
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
    
    private void OnAttackAnimationStarted()
    {
        int damageFromAttack = (currentComboStep < 3) ? softAttackDamage : heavyAttackDamage;
        weaponHitboxOnHand.ActivateWeaponHitbox(damageFromAttack);
        SoundFXManager.Instance.PlayRandomSoundFXAtPosition(SoundFXManager.Instance.swordSwingSounds,transform,0.15f);
    }
    private void OnAttackAnimationFinished()
    {
       // weaponHitboxOnHand.DisableWeaponHitbox();
    }
}
