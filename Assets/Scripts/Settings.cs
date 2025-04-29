using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    [Header("UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private GameObject settingsPanel;

    [Header("Behaviour")]
    [SerializeField] private bool pauseGame = true;

    private float cachedVol = -1f;

    private void Awake()
    {
        Instance = this;
        if (!settingsPanel) Debug.LogError("Settings: settingsPanel reference missing!");
        if (!musicSlider) Debug.LogError("Settings: musicSlider reference missing!");
    }

    private void Start()
    {
        musicSlider.value = SaveManager.Instance.MusicVol;
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
    }

    private void OnMusicChanged(float v)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVol(v);
        else
            cachedVol = v;
    }

    private void LateUpdate()
    {
        if (cachedVol >= 0f && AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVol(cachedVol);
            cachedVol = -1f;
        }
    }

    public void OpenSettings()
    {
        settingsPanel?.SetActive(true);

        if (pauseGame)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.InputLocked = true;  
            Time.timeScale = 0f;
        }
    }

    public void CloseSettings()
    {
        settingsPanel?.SetActive(false);

        if (pauseGame)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.InputLocked = false;
            Time.timeScale = 1f;
        }
    }

}
