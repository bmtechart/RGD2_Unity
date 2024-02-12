using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIController : MonoBehaviour
{
    #region Debug
    [SerializeField] private bool ShowDebug;
    [SerializeField] private GameObject AIDebugObject;
    #endregion

    #region Behaviour Componenets
    public Dictionary<string, AIBehaviour> Behaviours;


    [SerializeField] protected AIMovement aiMovement;
    [SerializeField] protected AIVision aiVision;
    [SerializeField] protected AIAttack aiAttack;
    #endregion

    #region Behaviour Tree
    protected BehaviourTree Tree;
    Node.Status treeStatus = Node.Status.RUNNING;
    #endregion

    // Awake is called when a script is loaded, before start
    private void Awake()
    {
        Behaviours = new Dictionary<string, AIBehaviour>();

        for(int i = 0; i < transform.childCount; i++)
        {
            
        }

        Tree = new BehaviourTree();
        if (!AIDebugObject)
        {
            Debug.Log("Warning, cannot debug AI Controller without AI Debug Object.");
            Debug.Log("Check prefabs folder of _Framework for PF_AIDebug.");
        }
    }


    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        treeStatus = Tree.Process();

        if(ShowDebug)
        {
            string DebugMessage = Tree.children[Tree.currentChild].name;
            AIDebugObject.GetComponent<TextMeshPro>().text = Tree.GetCurrentNodeStatus();
        }
    }
}
