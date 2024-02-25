using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndScreenController : WidgetController
{
    public override void Open()
    {
        base.Open();
        Debug.Log("Level end screen opened");
    }

    public void QuitGame()
    {
        Debug.Log("quit application");
        Application.Quit();

        if(Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
