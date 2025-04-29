using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (src == null)                               
            src = GetComponent<AudioSource>();         

        if (src == null)                               
            src = gameObject.AddComponent<AudioSource>();

        src.loop = true;
        DontDestroyOnLoad(src.gameObject);             

        float vol = (SaveManager.Instance != null)
            ? SaveManager.Instance.MusicVol
            : PlayerPrefs.GetFloat("Music", .8f);

        src.volume = Mathf.Clamp01(vol);
    }



    private void OnEnable()
    {
        if (Instance == null) Instance = this;
    }

    [Header("Music Source")]
    [SerializeField] private AudioSource src;          

    [Header("SFX Clips")]
    [SerializeField] private AudioClip clickSfx;
    [SerializeField] private AudioClip crashSfx;
    [SerializeField] private AudioClip winSfx;
    [SerializeField] private AudioClip loseSfx;


    public void SetMusicVol(float value)
    {
        if (src == null) { Debug.LogError("AudioManager: music AudioSource missing!"); return; }

        value = Mathf.Clamp01(value);
        src.volume = value;
        SaveManager.Instance.MusicVol = value;         
    }

    public void PlayMusic(AudioClip clip = null)
    {
        if (src == null) { Debug.LogError("music AudioSource missing"); return; }

        if (clip != null && src.clip != clip)
            src.clip = clip;

        src.volume = SaveManager.Instance.MusicVol;

        if (!src.isPlaying) src.Play();
    }

    public void StopMusic()
    {
        if (src == null) return;
        if (src.isPlaying) src.Stop();
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

        if (clip == null)
        {
            Debug.LogWarning($"AudioManager: SFX '{id}' not assigned!");
            return;
        }

        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }
}
