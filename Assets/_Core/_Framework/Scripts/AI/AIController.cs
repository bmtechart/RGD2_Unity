using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    protected BehaviourTree behaviourTree;
    Node.Status treeStatus = Node.Status.RUNNING;

    //stores all AI behaviours found on the parent game object. 
    //The goal is to use the GetBehaviour function instead of 
    protected Dictionary<Type, AIBehaviour> AIBehaviours;
    // Awake is called when a script is loaded, before start
    private void Awake()
    {
        //initialize AI behaviour dictionary
        AIBehaviours = new Dictionary<Type, AIBehaviour>();
        RegisterBehaviours();
    }

    /// <summary>
    /// Gets references to all the components of type AIBehaviour on the game object.
    /// Stores these components in a dictionary using their type as a key.
    /// </summary>
    private void RegisterBehaviours()
    {
        AIBehaviour[] behaviours = GetComponentsInChildren<AIBehaviour>();
        foreach (AIBehaviour b in behaviours)
        {
            AIBehaviours.Add(b.GetType(), b);
        }
    }

    /// <summary>
    /// AI Controller registers all AI behaviours on awake.
    /// This function returns the AI behaviour of the type provided. 
    /// </summary>
    /// <typeparam name="T">AIBehaviour type</typeparam>
    /// <returns></returns>
    protected T GetBehaviour<T>() where T : AIBehaviour
    {
        if (!AIBehaviours.ContainsKey(typeof(T))) return null;
        return (T) AIBehaviours[typeof(T)];
    } 

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (treeStatus != Node.Status.SUCCESS) treeStatus = behaviourTree.Process();
    }
}
