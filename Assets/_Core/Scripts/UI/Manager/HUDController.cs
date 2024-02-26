using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject losingGamePanel;
    public GameObject victoryPanel;
    public GameObject pausePanel;

    [Header("Text Displays")]
    public TextMeshProUGUI inGameScoreText; // For displaying score in-game
    public TextMeshProUGUI inGameTimerText; // For displaying timer in-game
    public TextMeshProUGUI enemyKillsText;

    [Header("Health UI")]
    public Slider sliderHealthBar; // Reference to the health bar slider
    public Color healthColorGreen = Color.green;
    public Color healthColorYellow = Color.yellow;
    public Color healthColorRed = Color.red;
    private Image healthBarFill; // To change the color of the health bar

    [Header("Report Card")]
    public TextMeshProUGUI reportCardEnemiesKilledText;
    public TextMeshProUGUI reportCardScoreText;
    public TextMeshProUGUI reportCardTimeText;


    private int score = 0;
    private float timer = 0f;
    private bool gameIsPaused = false;
    private float health = 100f; // Player's starting health
    public GameObject reportCardPanel; // Reference to the report card panel
    private int totalEnemiesKilled = 0;

    public LevelSelectionManager levelSelectionManager;

    void Start()
    {
        ResetGameUI();

        if (sliderHealthBar != null)
        {
            healthBarFill = sliderHealthBar.fillRect.GetComponent<Image>();
        }
        UpdateHealthBar();
    }

    void Update()
    {
        HandleGameTimer();
        CheckPauseInput();
    }

    public void EnemyKilled()
    {
        totalEnemiesKilled++;
        UpdateKillsDisplay();
    }
    void UpdateKillsDisplay()
    {
        if (enemyKillsText != null)
        {
            enemyKillsText.text = $"Kills: {totalEnemiesKilled}";
        }
    }

        public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();

        if (health <= 0)
        {
            PlayerLost(); // Assuming player's death means losing the game
        }
    }
    void UpdateHealthBar()
    {
        sliderHealthBar.value = health / 100f;
        UpdateHealthBarColor();
    }

    void UpdateHealthBarColor()
    {
        if (health > 50)
        {
            healthBarFill.color = healthColorGreen;
        }
        else if ((health > 25) && (health <= 50))
        {
            healthBarFill.color = healthColorYellow;
        }
        else
        {
            healthBarFill.color = healthColorRed;
        }
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
    void ShowReportCard()
    {
        UpdateReportCard(); // Assuming this method updates the Text elements within the report card
        reportCardPanel.SetActive(true); // Show the report card
    }

    public void PlayerLost()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
        ShowReportCard();

        losingGamePanel.SetActive(true);
    }

    public void PlayerWon()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
        ShowReportCard();
        victoryPanel.SetActive(true);
        // Unlock the next level using the LevelSelectionManager singleton
        LevelSelectionManager.Instance.UnlockNextLevel();
    }

    public void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting Level");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        Debug.Log("Loading Menu");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void LoadNextLevel()
    {
        int totalLevels = 4; 
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index exceeds the total number of levels
        if (nextSceneIndex < totalLevels)
        {
            Time.timeScale = 1f; // Make sure the game is not paused
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Optionally, load the main menu or show a game completion message
            
            SceneManager.LoadScene(0); // Load main menu or a 'Game Completed' scene
        }
    }
    void UpdateReportCard()
    {
        reportCardEnemiesKilledText.text = $"Enemies Killed: {totalEnemiesKilled}";
        reportCardScoreText.text = $"Score: {score}";
        reportCardTimeText.text = $"Time: {FormatTime(timer)}";
    }
}
