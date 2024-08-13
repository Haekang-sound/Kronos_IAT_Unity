using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree tree;
    private Context _context;

    void Start()
    {
        _context = CreateBehaviourTreeContext();
        tree = tree.Clone();
        //tree.Bind(_context);
        Bind();
    }

    void Update()
    {
        if(tree != null)
        {
            tree.Update();
        }
    }

    public void Bind()
    {
        tree.Bind(_context);
    }

    Context CreateBehaviourTreeContext()
    {
        return Context.CreateFromGameObject(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (!tree)
        {
            return;
        }

        BehaviorTree.Traverse(tree.rootNode, (n) => {
            if (n.drawGizmos)
            {
                n.OnDrawGizmos();
            }
        });
    }
}
