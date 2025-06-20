using UnityEngine;
using UnityEngine.Events;
public class Combatant : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float respawnDelayInSeconds = 5f;

    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider boxCollider;

    private float currentHealth;
    private bool isAlive = true;

    public bool IsAlive => isAlive;
    public float Health => currentHealth;
    public float MaxHealth => maxHealth;

    public float RespawnDelayInSeconds => respawnDelayInSeconds;

    public UnityEvent OnTakeDamage;
    public UnityEvent OnDie;
    public UnityEvent OnRespawn;

    void Start()
    {
        currentHealth = maxHealth;
        isAlive = true;

        if (animator == null)
            animator = GetComponent<Animator>();
        
        if(boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();
    }
    public void TakeDamage(float damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        animator.SetTrigger("Hit");

        OnTakeDamage?.Invoke();

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (!isAlive) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void Die()
    {
        isAlive = false;
        currentHealth = 0f;
        boxCollider.enabled = false;
        animator.SetTrigger("Die");


        OnDie?.Invoke();
        // Disable hitbox or collisions here if needed

        Invoke(nameof(Respawn), respawnDelayInSeconds);
    }

    void Respawn()
    {
        currentHealth = maxHealth;
        isAlive = true;

        animator.SetTrigger("Respawn");
        boxCollider.enabled = true;
        OnRespawn?.Invoke();
        // Re-enable collisions if needed
    }
}