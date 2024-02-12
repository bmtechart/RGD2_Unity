using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject losingGamePanel;
    public GameObject victoryPanel;
    public GameObject pausePanel;

    [Header("Text Displays")]
    public TextMeshProUGUI scoreText; // For displaying score on win/lose panels
    public TextMeshProUGUI timerText; // For displaying time on win/lose panels
    public TextMeshProUGUI inGameScoreText; // For displaying score in-game
    public TextMeshProUGUI inGameTimerText; // For displaying timer in-game

    private int score = 0;
    private float timer = 0f;
    private bool gameIsPaused = false;

    void Start()
    {
        ResetGameUI();
    }

    void Update()
    {
        HandleGameTimer();
        CheckPauseInput();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay(inGameScoreText, score);
    }

    void ResetGameUI()
    {
        score = 0;
        timer = 0f;
        gameIsPaused = false;
        UpdateScoreDisplay(inGameScoreText, score);
        UpdateTimerDisplay(inGameTimerText, timer);
        losingGamePanel.SetActive(false);
        victoryPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void HandleGameTimer()
    {
        if (!gameIsPaused)
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay(inGameTimerText, timer);
        }
    }

    void CheckPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void UpdateScoreDisplay(TextMeshProUGUI textMesh, int scoreValue)
    {
        textMesh.text = $"Score: {scoreValue}";
    }

    void UpdateTimerDisplay(TextMeshProUGUI textMesh, float timeValue)
    {
        textMesh.text = FormatTime(timeValue);
    }

    string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        return $"{minutes:00}:{seconds:00}";
    }

    public void PlayerLost()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
        UpdateScoreDisplay(scoreText, score);
        UpdateTimerDisplay(timerText, timer);
        losingGamePanel.SetActive(true);
    }

    public void PlayerWon()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
        UpdateScoreDisplay(scoreText, score);
        UpdateTimerDisplay(timerText, timer);
        victoryPanel.SetActive(true);
    }

    public void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}