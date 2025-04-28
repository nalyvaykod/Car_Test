using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    public Slider musicSlider, sfxSlider;
    [SerializeField] private GameObject MusicSliderGroup;
    [SerializeField] private GameObject BackButton;
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject StartGame;
    [SerializeField] private GameObject Setting;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        musicSlider.value = SaveManager.Instance.MusicVol;
    }

    public void GoToSettings()
    {
        Title.SetActive(false);
        StartGame.SetActive(false);
        Setting.SetActive(false);
        MusicSliderGroup.SetActive(true);
        BackButton.SetActive(true);
    }

    public void BackToMenu()
    {
        Title.SetActive(true);
        StartGame.SetActive(true);
        Setting.SetActive(true);
        MusicSliderGroup.SetActive(false);
        BackButton.SetActive(false);
    }

    public void OnMusicChanged(float v)
    {
        SaveManager.Instance.MusicVol = v;
        AudioManager.Instance.SetMusicVol(v);  
    }
}
