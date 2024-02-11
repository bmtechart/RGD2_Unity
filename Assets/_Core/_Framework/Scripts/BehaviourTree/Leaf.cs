using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick(); //one turnover for the behaviour tree
    public Tick ProcessMethod;

    //empty constructor
    public Leaf() { }
    public Leaf(string n, Tick pm) 
    {
        name = n;
        ProcessMethod = pm;
    }

    public override Status Process()
    {
        if (status == Status.RUNNING) { Debug.Log(name + " running!"); }
        if (ProcessMethod != null) return ProcessMethod();
        return Status.FAILURE;
    }
}
