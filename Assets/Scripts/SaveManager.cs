using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    const string COIN_KEY = "Coins";
    const string LVL_KEY = "Level";
    const string MUSIC_KEY = "Music";
    const string SFX_KEY = "Sfx";

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

    public int Coins => PlayerPrefs.GetInt(COIN_KEY, 0);
    public int CurrentLevel => PlayerPrefs.GetInt(LVL_KEY, 1);

    public float MusicVol
    {
        get => PlayerPrefs.GetFloat(MUSIC_KEY, .8f);
        set => PlayerPrefs.SetFloat(MUSIC_KEY, value);
    }

    public float SfxVol
    {
        get => PlayerPrefs.GetFloat(SFX_KEY, .8f);
        set => PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    public void LevelCompleted(int level, float time)
    {
        PlayerPrefs.SetInt(LVL_KEY, CurrentLevel + 1);
    }

    public void AddCoins(int amount)
    {
        int newTotal = Coins + amount;
        PlayerPrefs.SetInt(COIN_KEY, newTotal);
    }
}
