using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;  
    [SerializeField] private AudioClip clickSfx, crashSfx, winSfx, loseSfx;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(musicSource);
    }

    public void SetMusicVol(float v)
    {
        v = Settings.Instance.musicSlider.value;

        if (musicSource != null)
        {
            musicSource.volume = v;  
            Debug.Log("Set Music Volume: " + v);
        }
        else
        {
            Debug.LogError("MusicSource is not assigned!");
        }
    }

    public void PlaySfx(string id)
    {
        AudioClip clip = id switch
        {
            "Click" => clickSfx,
            "Crash" => crashSfx,
            "Win" => winSfx,
            _ => loseSfx
        };

        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
        else
        {
            Debug.LogError("SFX clip not assigned for id: " + id);
        }
    }

    public void PlayMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
        else if (musicSource == null)
        {
            Debug.LogError("MusicSource is not assigned!");
        }
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        else if (musicSource == null)
        {
            Debug.LogError("MusicSource is not assigned!");
        }
    }
}
