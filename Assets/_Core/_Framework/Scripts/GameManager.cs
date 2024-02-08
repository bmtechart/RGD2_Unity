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
    /// Delegate for when  
    /// </summary>
    public delegate void OnGameOver();

    public static event OnGameOver onGameOver;
    // Start is called before the first frame update

    override protected void Awake()
    {
        base.Awake();
        onGameOver += GameOver;
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
    protected virtual void GameOver()
    {

    }
}
