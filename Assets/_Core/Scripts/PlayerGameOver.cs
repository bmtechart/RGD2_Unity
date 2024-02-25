using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerGameOver : MonoBehaviour
{
    PlayerInput _input;
    public HealthBehaviour _health;
    // Start is called before the first frame update
    void Start()
    {
        if(!_input) _input = GetComponent<PlayerInput>();
        if(!_health) _health = GetComponentInChildren<HealthBehaviour>();   
        GameManager.Instance.OnGameOver.AddListener(OnGameOver);
        _health.OnDeath.AddListener(GameManager.Instance.GameOver);

    }

    void OnGameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if(_input) _input.enabled = false;
        WidgetManager.Instance.OpenWidget("LevelEndScreen");
    }
}
