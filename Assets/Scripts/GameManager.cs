using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-200)]
[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    [Header("Config")]
    [SerializeField] private int startLives = 3;

    [Header("UI")]
    [SerializeField] private Text livesTxt;
    [SerializeField] private Text timerTxt;
    [SerializeField] private Text levelTxt;
    [SerializeField] private Text coinTxt;
    [SerializeField] private Text winCoinsText;
    [SerializeField] private Text timeEndTxt;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject pausePanel;
    public int carsLeft { get; private set; }
    public bool InputLocked { get; set; }
    public float LevelTime => _timer;

    private int _lives;
    private int _levelIndex;
    private float _timer;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void OnEnable() => SceneManager.sceneLoaded += HandleSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= HandleSceneLoaded;

    private void HandleSceneLoaded(Scene s, LoadSceneMode m) => CacheUiRefs();

    private void Update()
    {
        _timer += Time.deltaTime;
        if (timerTxt) timerTxt.text = $"{_timer:0.0}";
    }

    public void SetupLevel(int level, int totalCars)
    {
        CacheUiRefs();

        _levelIndex = level;
        carsLeft = totalCars;
        _lives = startLives;
        _timer = 0f;

        InputLocked = false;
        Time.timeScale = 1f;
        pausePanel?.SetActive(false);
        AudioManager.Instance?.PlayMusic();

        SaveManager.Instance.AddCoins(-SaveManager.Instance.Coins);
        RefreshUI();
    }

    public void RegisterExit()
    {
        carsLeft--;
        SaveManager.Instance.AddCoins(_levelIndex * 5);
        if (!coinTxt) CacheUiRefs();
        if (coinTxt) coinTxt.text = $"{SaveManager.Instance.Coins}";

        if (carsLeft <= 0) Win();
    }

    public void CarCrashed()
    {
        carsLeft--;
        if (carsLeft <= 0) Lose();
    }

    public void LoseLife()
    {
        _lives--;
        if (livesTxt) livesTxt.text = $"{_lives}";
        if (_lives <= 0 || carsLeft <= 0) Lose();
    }

    public void Pause(bool state)
    {
        InputLocked = state;
        pausePanel.SetActive(state);
        Time.timeScale = state ? 0 : 1;
    }

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void LoadMenu() => SceneManager.LoadScene("MainMenu");

    private void Win()
    {
        CacheUiRefs();
        if (!winPanel) { Debug.LogError("WinPanel not found!"); return; }

        InputLocked = true;
        AudioManager.Instance?.StopMusic();
        AudioManager.Instance?.PlaySfx("Win");

        winPanel.SetActive(true);
        if (winCoinsText) winCoinsText.text = $"{SaveManager.Instance.Coins}";
        if (timeEndTxt) timeEndTxt.text = $"{_timer:0.0}s";
    }

    private void Lose()
    {
        CacheUiRefs();
        if (!losePanel) { Debug.LogError("LosePanel not found!"); return; }

        InputLocked = true;
        AudioManager.Instance?.StopMusic();
        AudioManager.Instance?.PlaySfx("Lose");

        losePanel.SetActive(true);
    }

    private void RefreshUI()
    {
        if (livesTxt) livesTxt.text = $"{_lives}";
        if (levelTxt) levelTxt.text = $"{_levelIndex}";
        if (timerTxt) timerTxt.text = "0.0";
        if (coinTxt) coinTxt.text = $"{SaveManager.Instance.Coins}";
    }

    private static Transform FindDeep(Scene scene, string name)
    {
        foreach (var root in scene.GetRootGameObjects())
        {
            var t = root.transform.Find(name);   
            if (t) return t;
        }
        return null;
    }

    private void CacheUiRefs()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!livesTxt) livesTxt = FindDeep(scene, "LivesText")?.GetComponent<Text>();
        if (!timerTxt) timerTxt = FindDeep(scene, "TimerText")?.GetComponent<Text>();
        if (!levelTxt) levelTxt = FindDeep(scene, "LevelText")?.GetComponent<Text>();
        if (!coinTxt) coinTxt = FindDeep(scene, "CoinText")?.GetComponent<Text>();
        if (!winCoinsText) winCoinsText = FindDeep(scene, "WinCoinsText")?.GetComponent<Text>();
        if (!timeEndTxt) timeEndTxt = FindDeep(scene, "TimeEndText")?.GetComponent<Text>();

        if (!winPanel) winPanel = FindDeep(scene, "WinPanel")?.gameObject;
        if (!losePanel) losePanel = FindDeep(scene, "LosePanel")?.gameObject;
        if (!pausePanel) pausePanel = FindDeep(scene, "PausePanel")?.gameObject;
    }
}
