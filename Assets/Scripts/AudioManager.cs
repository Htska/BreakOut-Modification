using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public AudioSource sfxSource;
    public AudioClip blockClip;

    private AudioSource musicSource;
    public AudioClip music;

    public AudioClip damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
            return;
        }
        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    private void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayBlockSFX() => PlaySFX(blockClip);

    public void PlayDamageSFX() => PlaySFX(damage);

    void Start()
    {
    }

    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.pitch = 1f;
        musicSource.Play();
    }

    public void MusicLowPitch(float pitch)
    {
        musicSource.pitch = pitch;
    }

    public void EndSong()
    {
        musicSource.Stop();
    }

}
