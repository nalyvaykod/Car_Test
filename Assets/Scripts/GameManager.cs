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

    public void RegisterExit(CarController car)
    {
        Destroy(car.gameObject);
        carsLeft--;
        if (carsLeft <= 0) Win();
    }

    public void LoseLife()
    {
        lives--;
        livesTxt.text = $"{lives}";
        if (lives <= 0) Lose();
    }

    void Win()
    {
        
    }
}
