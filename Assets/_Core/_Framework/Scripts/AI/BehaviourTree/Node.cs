using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Node
{
    //properties
    public enum Status { SUCCESS, RUNNING, FAILURE };
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public Node runningProcess;
    public Node behaviourTree;
    public string name;


    //constructors
    public Node() { }
    public Node(string n)
    {
        name = n;
    }
    
    //methods
    public virtual Status Process() 
    {
        if (status == Status.RUNNING) { Debug.Log(name + " running!"); }
        return children[currentChild].Process(); 
    }  
    public virtual void AddChild(Node n) { children.Add(n); }
}
