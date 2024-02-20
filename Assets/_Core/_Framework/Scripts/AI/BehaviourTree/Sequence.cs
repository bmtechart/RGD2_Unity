using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string n) { name = n; }

    //the sequence node will return success if all of its children are succesful
    //if any child fails, the sequence node fails
    public override Status Process()
    {
        Status childstatus = children[currentChild].Process();
        if(childstatus == Status.RUNNING) return Status.RUNNING;
        if (childstatus == Status.FAILURE) return Status.FAILURE;

        currentChild++;
        //all children have looped through and been successful
        if (currentChild >= children.Count) 
        { 
            currentChild = 0;
            return Status.SUCCESS;
        }
        return Status.RUNNING;
    }

    public override void AddChild(Node n)
    {
        base.AddChild(n);
        n.behaviourTree = behaviourTree;
    }

}
