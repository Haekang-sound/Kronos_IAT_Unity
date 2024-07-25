using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : Node
{
    public Node child;
    protected override void OnStart() 
    {
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    { 
        return child.Update();
    }

    public override Node Clone()
    {
        Start node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
