using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    const string BEST_KEY = "BestTime", COIN_KEY = "Coins", LVL_KEY = "Level", MUSIC_KEY = "Music", SFX_KEY = "Sfx";

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

    public float BestTime => PlayerPrefs.GetFloat(BEST_KEY, float.MaxValue);
    public string BestTimePretty => BestTime == float.MaxValue ? "-" : $"{BestTime: 0.0}s";
    public int Coins => PlayerPrefs.GetInt(COIN_KEY, 0);
    public int CurrentLevel => PlayerPrefs.GetInt(LVL_KEY, 1);

    public float MusicVol
    {
        get => PlayerPrefs.GetFloat(MUSIC_KEY, .8f);
        set { PlayerPrefs.SetFloat(MUSIC_KEY, value); }
    }

    public float SfxVol
    {
        get => PlayerPrefs.GetFloat(SFX_KEY, .8f);
        set { PlayerPrefs.SetFloat(SFX_KEY, value); }
    }

    public void LevelCompleted(int level, float time)
    {
        PlayerPrefs.SetInt(LVL_KEY, level + 1);
        int coins = Coins + level * 5;
        PlayerPrefs.SetInt(COIN_KEY, coins);
        if (time < BestTime)
        {
            PlayerPrefs.SetFloat(BEST_KEY, time);
        }
    }

}
