using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector (string n) { name = n; }

    public override Status Process()
    {
        Status childstatus = children[currentChild].Process();

        if(childstatus == Status.RUNNING) return Status.RUNNING;
        
        if (childstatus == Status.SUCCESS)
        {
            currentChild = 0; //RESET
            return childstatus;
        }

        currentChild++;

        if(currentChild>=children.Count)
        {
            currentChild = 0; //Reset
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }

    public override void AddChild(Node n)
    {
        base.AddChild(n);
        n.behaviourTree = behaviourTree;
    }
}