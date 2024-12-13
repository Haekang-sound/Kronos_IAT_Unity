using UnityEditor;
using UnityEngine;

/// <summary>
/// 'Context' 클래스는 모든 노드가 접근할 수 있는 공유 객체로, 
/// 일반적으로 사용되는 컴포넌트와 시스템을 저장하는 데 사용됩니다.
/// </summary>
public class Context
{
    public GameObject gameObject;
    public Transform transform;
    public Animator animator;
    public Rigidbody physics;
    public UnityEngine.AI.NavMeshAgent agent;
    public SphereCollider sphereCollider;
    public BoxCollider boxCollider;
    public CapsuleCollider capsuleCollider;
    public CharacterController characterController;

    /// <summary>
    /// 주어진 게임 오브젝트로부터 Context 객체를 생성합니다.
    /// 게임 오브젝트에 관련된 여러 컴포넌트를 가져와서 Context에 저장합니다.
    /// </summary>
    /// <param name="gameObject">컴포넌트를 가져올 게임 오브젝트</param>
    /// <returns>생성된 Context 객체</returns>
    public static Context CreateFromGameObject(GameObject gameObject)
    {
        // 일반적으로 사용되는 컴포넌트를 가져와 Context 객체에 저장
        Context context = new Context();
        context.gameObject = gameObject;
        context.transform = gameObject.transform;
        context.animator = gameObject.GetComponent<Animator>();
        context.physics = gameObject.GetComponent<Rigidbody>();
        context.agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        context.sphereCollider = gameObject.GetComponent<SphereCollider>();
        context.boxCollider = gameObject.GetComponent<BoxCollider>();
        context.capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        context.characterController = gameObject.GetComponent<CharacterController>();

        // 필요에 따라 추가적인 시스템을 이곳에 추가할 수 있습니다...

        return context;
    }
}