using UnityEngine;
using UnityEngine.UI;

public class RespawnBar : MonoBehaviour
{
    [SerializeField] private Combatant target;
    [SerializeField] private Image fillImage;

    private float _cooldownDuration;
    private float _timer;
    private bool _isRespawnTimerStarted;
    void Update()
    {
        if (!_isRespawnTimerStarted) return;

        _timer += Time.deltaTime;
        float progress = Mathf.Clamp01(_timer / _cooldownDuration);
        fillImage.fillAmount = 1f - progress;

        if (_timer >= _cooldownDuration)
            StopCooldown();
    }
    public void StartCooldown()
    {
        _cooldownDuration = target.RespawnDelayInSeconds;
        _timer = 0f;
        _isRespawnTimerStarted = true;
        fillImage.fillAmount = 1f;
    }
    public void StopCooldown()
    {
        _isRespawnTimerStarted = false;
        fillImage.fillAmount = 0f;
    }
}