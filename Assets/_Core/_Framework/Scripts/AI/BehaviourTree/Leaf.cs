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
        status = Status.FAILURE;
        if (ProcessMethod != null)
        {
            status = ProcessMethod();
            behaviourTree.runningProcess = this;
            return status;
        }
        return status;
    }

    public override void AddChild(Node n)
    {
        base.AddChild(n);
    }
}
