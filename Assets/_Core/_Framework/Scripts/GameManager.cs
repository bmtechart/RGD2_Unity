using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The game manager is tasked with defining and implementing the rules of the game.
/// </summary>
public class GameManager : Singleton<GameManager>
{

    /// <summary>
    /// Delegate for when the player loses the game.
    /// </summary>
    public delegate void OnGameOver();

    public static event OnGameOver onGameOver;

    /// <summary>
    /// Event for when the player completes a level. 
    /// </summary>
    public delegate void OnLevelEnd();
    public static event OnLevelEnd onLevelEnd;
    // Start is called before the first frame update

    override protected void Awake()
    {
        base.Awake();
    }

    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    /// <summary>
    /// Function to call when the game session ends. 
    /// Override to add default functionality when game ends. 
    /// </summary>
    public virtual void GameOver()
    {
        onGameOver?.Invoke();
    }

    public virtual void LevelEnd()
    {
        onLevelEnd?.Invoke();   
    }
}
