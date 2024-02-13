using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePauseMenu : WidgetController
{
    public override void Open()
    {
        base.Open();

        //set active to make widget visible
        gameObject.SetActive(true);

        //show cursor
        Cursor.visible = true;

        //allow user to move cursor on screen
        Cursor.lockState = CursorLockMode.Confined;

        //pause game
        Time.timeScale = 0.0f;
 
    }

    public override void Close()
    {
        base.Close();

        //hide cursor
        Cursor.visible = false;

        //lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;

        //resume time
        Time.timeScale = 1.0f;

        //hide widget to make invisible
        gameObject.SetActive(false);

    }

    public override void Start()
    {
        base.Start();
   
        GameObject widget = WidgetManager.Instance.OpenWidget(Name);
    }

    public void Resume()
    {
        WidgetManager.Instance.CloseWidget(Name);
    }
}
