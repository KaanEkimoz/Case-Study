using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHitbox : MonoBehaviour
{
    [SerializeField] private float weaponBaseDamage = 10f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private BoxCollider boxCollider;

    private float _additionalDamage;
    private List<Collider> _alreadyHit = new();

    public UnityEvent OnHit;
    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyHit.Contains(other)) return;

        if(other.TryGetComponent<IDamageable>(out IDamageable hitdamageable))
        {
            hitdamageable.TakeDamage(weaponBaseDamage + _additionalDamage);
            
            OnHit?.Invoke();
            Debug.Log("Damageable HIT");
        }
        _alreadyHit.Add(other);
    }
    public void ActivateWeaponHitbox(float extraDamageFromOtherSources)
    {
        _alreadyHit.Clear();
        _additionalDamage = extraDamageFromOtherSources;
        boxCollider.enabled = true;
        
    }
    public void DisableWeaponHitbox()
    {
        _alreadyHit.Clear();
        _additionalDamage = 0;
        boxCollider.enabled = false;
        
    }
}

