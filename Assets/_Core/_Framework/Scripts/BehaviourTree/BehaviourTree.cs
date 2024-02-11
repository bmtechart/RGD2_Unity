using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
    #region Constructors
    public BehaviourTree()
    {
        name = "Tree";
    }

    public BehaviourTree(string n)
    {
        name = n;
    }
    #endregion

    #region Methods

    public override Status Process()
    {
        return children[currentChild].Process();
    }

    #endregion

    #region Debugging
    struct NodeLevel
    {
        public int level;
        public Node node;
    }

    public void PrintTree()
    {
        string treePrintout = "";
        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        Node currentNode = this;
        nodeStack.Push( new NodeLevel { level = 0, node = currentNode } );

        while(nodeStack.Count != 0)
        {
            NodeLevel nextNode = nodeStack.Pop();
            treePrintout += new string('-', nextNode.level) + nextNode.node.name + "\n";
            for(int i = nextNode.node.children.Count - 1; i>=0; i--)
            {
                nodeStack.Push(new NodeLevel { level = nextNode.level+1, node = nextNode.node.children[i] });
            }
        }

        Debug.Log(treePrintout);
    }

    public string GetCurrentNodeStatus()
    {
        string nodePrintout = "";

        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        Node currentNode = this;
        nodeStack.Push(new NodeLevel { level = 0, node = currentNode });

        while (nodeStack.Count != 0)
        {
            NodeLevel nextNode = nodeStack.Pop();
            for (int i = nextNode.node.children.Count - 1; i >= 0; i--)
            {
                nodeStack.Push(new NodeLevel { level = nextNode.level + 1, node = nextNode.node.children[i] });
            }
        }

        foreach (NodeLevel nodeLevel in nodeStack)
        {
            if(nodeLevel.node.status == Node.Status.RUNNING)
            {
                nodePrintout += nodeLevel.node.name + " running!";
            }
        }

        return nodePrintout;
    }
    #endregion
}
