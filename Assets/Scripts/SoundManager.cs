using UnityEngine;

public enum MainSfx
{
    KeyPress = 0,
    StartWawa = 1,
    Monkey = 2,
    Valid = 3,
    Wrong = 4
}

public class SoundManager : MonoBehaviour
{
        
    public static SoundManager Instance;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] sfx;

    [Header("Audio Sources")] 
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
            
        Instance = this;
    }

    private void Start()
    {
        SoundPlay(MainSfx.StartWawa);
    }
    
    public void OnInputValueChanged(string value)
    {
        SoundPlay(MainSfx.KeyPress);
    }
        
    public void SoundPlay(MainSfx sound)
    {
        AudioClip soundToPlay = sfx[(int)sound];
        sfxSource.PlayOneShot(soundToPlay);
    }
}