using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Combatant target;
    [SerializeField] Image fillImage;
    //[SerializeField] Vector3 offset = Vector3.up * 2f;

    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (target == null || fillImage == null) return;

        float normalizedHealth = target.Health / target.MaxHealth;
        fillImage.fillAmount = normalizedHealth;

        //transform.position = target.transform.position + offset;
    }
}
