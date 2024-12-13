using UnityEngine;

/// <summary>
/// 행동 트리를 실행하고 업데이트하는 클래스입니다.
/// 이 클래스는 주어진 행동 트리를 실행하고, 트리의 상태를 주기적으로 갱신합니다.
/// </summary>
public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree tree;
    public bool play = true;

    private Context _context;

    void Start()
    {
        _context = CreateBehaviourTreeContext();
        BindTree();
    }

    void Update()
    {
        if(tree != null && play == true)
        {
            tree.Update();
        }
    }

    public void Bind()
    {
        tree.Bind(_context);
    }

    public void BindTree()
    {
        if (tree == null) return;

        tree = tree.Clone();
        Bind();
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
