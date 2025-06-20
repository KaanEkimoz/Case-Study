using UnityEngine;

public class Dummy : MonoBehaviour
{
    public void OnDummyFallAnimationEvent()
    {
        SoundFXManager.Instance.PlayRandomSoundFXAtPosition(SoundFXManager.Instance.dummyFallSounds, transform, 0.15f);
    }
    public void OnDummyDamagedAnimationEvent()
    {
        SoundFXManager.Instance.PlayRandomSoundFXAtPosition(SoundFXManager.Instance.dummyDamagedSounds, transform, 0.15f);
    }
}
