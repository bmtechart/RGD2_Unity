using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private HealthBehaviour healthBehaviour;
    private PlayerInput _input;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _input = GetComponent<PlayerInput>();
        if (GameManager.Instance)
        {

            gameManager = GameManager.Instance;
            gameManager.OnLevelComplete.AddListener(OnPlayerCompleteLevel);

            healthBehaviour = GetComponentInChildren<HealthBehaviour>();

            healthBehaviour.OnDeath.AddListener(OnPlayerDeath);
        }
    }



    public void OnPlayerDeath() 
    {
        Debug.Log("player death!");
        gameManager.GameOver();
        _input.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        WidgetManager.Instance.OpenWidget("LevelEndScreen");
    }

    public void OnPlayerCompleteLevel() 
    {
        _input.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
