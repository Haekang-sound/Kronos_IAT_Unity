using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �� ��ȯ ��, �÷��̾� �Ǵ� Ư�� ���� ������Ʈ�� �̵��� �������� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class SceneTransitionDestination : MonoBehaviour
{
    [Tooltip("�÷��̾�� ���� ���� ���� �Ű��� ���� ������Ʈ �Դϴ�.")]
    public GameObject transitioningGameObject;
    public UnityEvent OnReachDestination;
}