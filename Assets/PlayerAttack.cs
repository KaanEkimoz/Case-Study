using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] float comboResetTime = 1.2f;
    [SerializeField] float inputWindow = 0.6f;

    [Header("Damage Values")]
    [SerializeField] int softAttackDamage = 10;
    [SerializeField] int heavyAttackDamage = 30;

    [Header("References")]
    [SerializeField] Animator animator;

    int currentComboStep = 0;
    float lastAttackTime;
    bool canChainNextAttack;

    void Update()
    {
        if (PlayerInputHandler.Instance.AttackButtonPressed)
            TryAttack();

        if (Time.time - lastAttackTime > comboResetTime)
            ResetCombo();
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
        ApplyDamage(softAttackDamage);
        Invoke(nameof(EnableNextAttack), inputWindow);
    }

    void ChainAttack()
    {
        currentComboStep++;
        lastAttackTime = Time.time;
        canChainNextAttack = false;

        string trigger = "Attack" + currentComboStep;
        animator.SetTrigger(trigger);

        int damage = (currentComboStep < 3) ? softAttackDamage : heavyAttackDamage;
        ApplyDamage(damage);

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

    void ApplyDamage(int amount)
    {
        // Implement dummy damage logic here later
        Debug.Log($"Dealt {amount} damage");
    }
}
