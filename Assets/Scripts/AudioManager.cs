using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer mixer;
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
    }

    public void SetMusicVol(float v) => mixer.SetFloat("MusicVol", Mathf.Log10(v) * 20);
    public void SetSfxVol(float v) => mixer.SetFloat("SfxVol", Mathf.Log10(v) * 20);

    public void PlaySfx(string id)
    {
        var clip = id switch
        {
            "Click" => clickSfx,
            "Crasg" => crashSfx,
            "Win" => winSfx,
            _ => loseSfx
        };
        AudioSource.PlayClipAtPoint(clip, Vector3.zero);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
