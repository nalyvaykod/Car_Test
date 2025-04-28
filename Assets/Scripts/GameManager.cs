using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int startLives = 3;
    [SerializeField] private Text livesTxt, timerTxt, levelTxt, coinTxt, winCoinsText, timeEndTxt;
    [SerializeField] private GameObject winPanel, losePanel, pausePanel;
    public int carsLeft;

    private int lives, levelIndex;
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
        Debug.Log("Cars Left set to: " + carsLeft);
        lives = startLives;
        timer = 0f;
        SaveManager.Instance.AddCoins(-SaveManager.Instance.Coins);
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
        int coinsEarned = levelIndex * 5;
        SaveManager.Instance.AddCoins(coinsEarned);
        coinTxt.text = $"{SaveManager.Instance.Coins}";
        if (carsLeft <= 0)
        {
            Win();
        }
    }

    public void LoseLife()
    {
        lives--;
        livesTxt.text = $"{lives}";

        if (lives <= 0 || carsLeft <= 0)
        {
            Lose();
        }
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
        Debug.Log("Win() called!");

        if (winPanel != null)
        {
            InputLocked = true;
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySfx("Win");

            winPanel.SetActive(true);
            winCoinsText.text = $"{SaveManager.Instance.Coins}";

            timeEndTxt.text = $"{timer:0.0}s"; 
        }
        else
        {
            Debug.LogError("Win Panel is not assigned!");
        }
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
        coinTxt.text = $"{SaveManager.Instance.Coins}";
    }
}
