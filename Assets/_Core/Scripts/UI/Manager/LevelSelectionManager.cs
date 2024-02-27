using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    public static LevelSelectionManager Instance { get; private set; }
    public GameObject levelSelectionPanel;
    public Button[] levelButtons;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        int levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);
        UnlockLevels(levelsUnlocked);
    }

    void UnlockLevels(int levels)
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = i < levels;
        }
    }

    public void SelectLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void UnlockNextLevel()
    {
        int levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);
        levelsUnlocked++;
        PlayerPrefs.SetInt("levelsUnlocked", levelsUnlocked);
        UnlockLevels(levelsUnlocked);
    }
}
