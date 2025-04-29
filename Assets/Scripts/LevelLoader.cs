using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadNextLevel()
    {
        SaveManager sm = SaveManager.Instance;

        sm.LevelCompleted(sm.CurrentLevel, GameManager.Instance.LevelTime);
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        AudioManager.Instance.SetMusicVol(SaveManager.Instance.MusicVol);
        AudioManager.Instance.PlayMusic();

        AudioManager.Instance.StopMusic();

        SceneManager.LoadScene("MainMenu");
    }
}
