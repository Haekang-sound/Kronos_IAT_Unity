using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneTransitionDestination : MonoBehaviour
{
    [Tooltip("플레이어와 같이 다음 씬에 옮겨질 게임 오브젝트 입니다.")]
    public GameObject transitioningGameObject;
    public UnityEvent OnReachDestination;
}