using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic type for singleton pattern.
/// Abstract designation means default singleton can never be implemented.
/// Only child classes of this type may be implemented. 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> 
{
    private static T _instance;

    /// <summary>
    /// Return the main isntance of the singleton.
    /// </summary>
    public static T Instance
    {
        get 
        { 

        if (_instance == null)
            {
                Debug.Log(typeof(T).ToString() + " is missing!");
            }
                
        return _instance;
        }
    }

    private void Awake()
    {
        if(_instance)
        {
            Debug.LogWarning("Second instance of " + typeof(T) + " created. Destroyed first isntance to prevent conflict.");
            Destroy(this.gameObject);
        }

        _instance = this as T;

        Init();
    }

    /// <summary>
    /// Override this method to add behaviour when a new instance of the singleton is first created. 
    /// </summary>
    public virtual void Init()
    {

    }

    private void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
    
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
