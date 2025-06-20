using UnityEngine;
public class SoundFXManager : MonoBehaviour
{
    #region Singleton
    public static SoundFXManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;
    [Space]
    [Header("Player Sounds")]
    [SerializeField] public AudioClip[] playerWalkOnConcreteSounds;
    [Space]
    [Header("Sword Sounds")]
    [SerializeField] public AudioClip[] swordSwingSounds;
    [SerializeField] public AudioClip[] swordHitSounds;
    [Space]
    [Header("Dummy Sounds")]
    [SerializeField] public AudioClip[] dummyDamagedSounds;
    [SerializeField] public AudioClip[] dummyFallSounds;
    [SerializeField] public AudioClip[] dummyRespawnedSounds;
    public void PlaySoundFXAtPosition(AudioClip audioClip, Transform spawnTransform, float volume = 0.5f)
    {
        AudioSource tempAudioSource = Instantiate(audioSource, spawnTransform.position, Quaternion.identity);
        this.audioSource.clip = audioClip;
        this.audioSource.volume = volume;
        this.audioSource.Play();
        float clipLength = audioClip.length;
        Destroy(tempAudioSource.gameObject, clipLength);
    }
    public void PlayRandomSoundFXAtPosition(AudioClip[] audioClip, Transform spawnTransform, float volume = 0.5f)
    {
        AudioSource tempAudioSource = Instantiate(audioSource, spawnTransform.position, Quaternion.identity);

        int randomIndex = Random.Range(0, audioClip.Length);
        tempAudioSource.clip = audioClip[randomIndex];
        tempAudioSource.volume = volume;
        tempAudioSource.Play();
        float clipLength = audioClip[randomIndex].length;
        Destroy(tempAudioSource.gameObject, clipLength);
    }
}
