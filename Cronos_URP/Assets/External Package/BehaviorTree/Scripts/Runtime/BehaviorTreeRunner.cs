using UnityEngine;

/// <summary>
/// �ൿ Ʈ���� �����ϰ� ������Ʈ�ϴ� Ŭ�����Դϴ�.
/// �� Ŭ������ �־��� �ൿ Ʈ���� �����ϰ�, Ʈ���� ���¸� �ֱ������� �����մϴ�.
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
