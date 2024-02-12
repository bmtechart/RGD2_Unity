using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetController : MonoBehaviour, IUserInterface
{    
     /// <summary>
     /// Name is used to store the name of this user interface
     /// </summary>
    public string Name;

    /// <summary>
    /// Register widget to UI manager.
    /// </summary>
    private void Awake()
    {
        gameObject.SetActive(false);
        if (UIManager.Instance)
        {
            UIManager.Instance.RegisterWidget(Name, gameObject);
            return;
        } 
        else
        {
            Debug.LogWarning("No Instance of UI Manager in scene!");
        }
    }

    /// <summary>
    /// Call open to open the widget. This can contain animations, sound effects, whatever.
    /// </summary>
    public virtual void Open()
    {

    }

    /// <summary>
    /// Implement this function to close the wigdet. Put any animations, sound effects, pausing, etc here. 
    /// </summary>
    public virtual void Close()
    {

    }

    /// <summary>
    /// Call reset to reset widget to its initial state. 
    /// </summary>
    public virtual void Reset()
    {

    }
}
