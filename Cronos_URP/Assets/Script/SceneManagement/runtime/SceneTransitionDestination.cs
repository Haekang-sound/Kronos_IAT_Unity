using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 씬 전환 시, 플레이어 또는 특정 게임 오브젝트가 이동할 목적지를 설정하는 클래스입니다.
/// </summary>
public class SceneTransitionDestination : MonoBehaviour
{
    [Tooltip("플레이어와 같이 다음 씬에 옮겨질 게임 오브젝트 입니다.")]
    public GameObject transitioningGameObject;
    public UnityEvent OnReachDestination;
}