using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Combatant target;
    [SerializeField] private Image fillImage;

    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
        fillImage.fillAmount = 1;
    }
    public void UpdateHealthBar()
    {
        if (target == null || fillImage == null) return;

        float normalizedHealth = target.Health / target.MaxHealth;
        fillImage.fillAmount = normalizedHealth;
    }
}
