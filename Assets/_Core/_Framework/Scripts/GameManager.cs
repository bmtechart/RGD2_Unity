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
    public UnityEvent OnGameOver;

    /// <summary>
    /// Event for when the player completes a level. 
    /// </summary>
    public UnityEvent OnLevelComplete;

    /// <summary>
    /// Event for when the level begins. Not to be confused with Start() which is called when the script is created. 
    /// OnLevelStart should be invoked when the player actually starts playing the game.
    /// </summary>
    public UnityEvent OnLevelStart;
   



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
        OnGameOver?.Invoke();
    }

    public virtual void LevelComplete()
    {
        OnLevelComplete?.Invoke();   
    }

    public virtual void LevelStart()
    {
        OnLevelStart?.Invoke();
    }
}
