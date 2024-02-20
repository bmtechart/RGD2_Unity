using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelLoader : MonoBehaviour
{
    private AsyncOperation loadOperation;
    [SerializeField]
    private UnityEngine.UI.Slider progressBar;
    // Progress values.
    private float currentValue;
    private float targetValue;
    // Multiplier for progress animation speed.
    [SerializeField]
    [Range(0, 1)]
    private float progressAnimationMultiplier = 0.25f;

    private void Start()
    {
        // Set 0 for progress values.
        progressBar.value = currentValue = targetValue = 0;
        // Load the next scene.
        var currentScene = SceneManager.GetActiveScene();
        loadOperation = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);

        // With this flag set, progress will stop at 0.9f.
        loadOperation.allowSceneActivation = false;
    }

    private void Update()
    {
        // Assign current load progress, divide by 0.9f to stretch it to values between 0 and 1.
        targetValue = loadOperation.progress / 0.9f;

        // Calculate progress value to display.
        currentValue = Mathf.MoveTowards(currentValue, targetValue, progressAnimationMultiplier * Time.deltaTime);
        progressBar.value = currentValue;

        // When the progress reaches 1, allow the process to finish by setting the scene activation flag.
        if (Mathf.Approximately(currentValue, 1))
        {
            loadOperation.allowSceneActivation = true;
        }
    }
}