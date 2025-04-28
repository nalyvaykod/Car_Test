using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int startLives = 3;
    [SerializeField] private Text livesTxt, timerTxt, levelTxt, coinTxt;
    [SerializeField] private GameObject winPanel, losePanel, pausePanel;

    private int lives, carsLeft, levelIndex;
    private float timer;
    public bool InputLocked { get; private set; }

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

    public void SetupLevel(int level, int totalCars)
    {
        levelIndex = level;
        carsLeft = totalCars;
        lives = startLives;
        timer = 0f;
        RefreshUI();
    }

    void Update()
    {
        timer += Time.deltaTime;
        timerTxt.text = $"{timer: 0.0}";
    }

    public void RegisterExit()
    {
        carsLeft--;
        if (carsLeft <= 0) Win();
    }

    public void LoseLife()
    {
        lives--;
        livesTxt.text = $"{lives}";
        if (lives <= 0) Lose();
    }

    public void Pause(bool state)
    {
        InputLocked = state;
        pausePanel.SetActive(state);
        Time.timeScale = state ? 0 : 1;
    }

    public void RestartLevel() => SceneManager.LoadScene("Game");
    public void LoadMenu() => SceneManager.LoadScene("MainMenu");

    void Win()
    {
        InputLocked = true;
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySfx("Win");
        SaveManager.Instance.LevelCompleted(levelIndex, timer);
        winPanel.SetActive(true);
        coinTxt.text = $"+{levelIndex * 5}";
    }

    void Lose()
    {
        InputLocked = true;
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySfx("Lose");
        losePanel.SetActive(true);
    }

    void RefreshUI()
    {
        livesTxt.text = $"{lives}";
        levelTxt.text = $"{levelIndex}";
        timerTxt.text = "0.0";
    }
}
