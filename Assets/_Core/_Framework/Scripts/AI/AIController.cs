using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class AIController : MonoBehaviour
{
    #region Debug
    [SerializeField] private bool ShowDebug;

    [Tooltip("The location of the debug message in world space.")]
    [SerializeField] private Transform DebugRoot;
    #endregion

    #region Behaviour Componenets
    public Dictionary<string, AIBehaviour> Behaviours = new Dictionary<string, AIBehaviour>();

    #endregion

    #region Behaviour Tree
    protected BehaviourTree Tree;
    /*
    Node.Status treeStatus = Node.Status.RUNNING;
    */
    #endregion

    public Animator Animator;

    // Awake is called when a script is loaded, before start
    private void Awake()
    {
        Tree = new BehaviourTree();
    }

    public virtual void DebugBehaviourTree()
    {
        if (Tree == null) return;
        if (ShowDebug && Tree.runningProcess != null)
        {
            string DebugMessage = Tree.runningProcess.name + Tree.runningProcess.status;

            Vector3 DebugMessageLocation = new Vector3();

            DebugMessageLocation = transform.position;

            if (DebugRoot) DebugMessageLocation = DebugRoot.position;

            Handles.Label(DebugMessageLocation, DebugMessage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; 
        if (ShowDebug) DebugBehaviourTree();
    }

    public void RegisterBehaviour(string name, AIBehaviour behaviour)
    {
        AIBehaviour b;
        if(Behaviours.TryGetValue(name, out b))
        {
            Debug.Log("Only one behaviour with name " + name + " can be registered at a time.");
            return;
        }

        Behaviours.Add(name, behaviour);
        behaviour.Animator = Animator;
    }

    public T GetAIBehaviour<T>(string name) where T : AIBehaviour
    {
        AIBehaviour b;
        if(Behaviours.TryGetValue(name, out b))
        {
            return b as T;
        }

        Debug.Log("No behaviour registered with name " + name + ".");
        return null;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        if (Animator) InitializeAnimators();
    }

    /// <summary>
    /// Sets the animator component of all registered AIBehaviours on this game object
    /// </summary>
    private void InitializeAnimators()
    {
        foreach (AIBehaviour ai in Behaviours.Values)
        {
            ai.Animator = Animator;
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //treeStatus = Tree.Process();

        
    }
}
