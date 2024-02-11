using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework 
{
    /// <summary>
    /// The game instance is an object that exists
    /// It is a persistent object that exists between levels. 
    /// </summary>
    public class GameInstance : Singleton<GameInstance>
    {
        /// <summary>
        /// Generate a persistent level to contain the game instance object. 
        /// Runtime Initialize On Load Method is executed when the application loads.
        /// When application loads, load the persistent level.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadPersistentLevel()
        {
            const string LevelName = "PersistentLevel";

            //loop through currently loaded scene. 
            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                //if the persistent scene has already been loaded, exit
                if (SceneManager.GetSceneAt(sceneIndex).name == LevelName) return;
            }

            //append the persistent level to the loaded levels
            SceneManager.LoadScene(LevelName, LoadSceneMode.Additive);
        }

        public override void Init()
        {
            base.Init();
            DontDestroyOnLoad(gameObject);
        }
    }
}
