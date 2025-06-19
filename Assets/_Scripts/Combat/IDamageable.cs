public interface IDamageable
{
    bool IsAlive { get; }
    float Health { get; }
    float MaxHealth { get; }

    void TakeDamage(float damage);
    void Heal(float amount);

    void Die();
}
