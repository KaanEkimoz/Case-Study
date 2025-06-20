using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tiny;

public class WeaponHitbox : MonoBehaviour
{
    [SerializeField] private float weaponBaseDamage = 10f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Trail trail;
    [SerializeField] private GameObject hitParticle;

    private float _additionalDamage;
    private List<Collider> _alreadyHit = new();
    
    public UnityEvent OnHit;
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;
    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyHit.Contains(other)) return;

        if(other.TryGetComponent<IDamageable>(out IDamageable hitdamageable))
        {
            hitdamageable.TakeDamage(weaponBaseDamage + _additionalDamage);

            Instantiate(hitParticle, other.ClosestPoint(transform.position), Quaternion.identity);
            OnHit?.Invoke();
            SoundFXManager.Instance.PlayRandomSoundFXAtPosition(SoundFXManager.Instance.swordHitSounds, transform, 0.15f);
            Debug.Log("Damageable HIT");
        }
        _alreadyHit.Add(other);
    }
    public void ActivateWeaponHitbox(float extraDamageFromOtherSources, bool isHeavyAttack = false)
    {
        _alreadyHit.Clear();
        _additionalDamage = extraDamageFromOtherSources;
        boxCollider.enabled = true;
        OnActivated?.Invoke();
    }
    public void DisableWeaponHitbox()
    {
        _alreadyHit.Clear();
        _additionalDamage = 0;
        boxCollider.enabled = false;
        OnDeactivated?.Invoke();
    }
}

