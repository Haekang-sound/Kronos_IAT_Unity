using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneTransitionDestination : MonoBehaviour
{
    [Tooltip("�÷��̾�� ���� ���� ���� �Ű��� ���� ������Ʈ �Դϴ�.")]
    public GameObject transitioningGameObject;
    public UnityEvent OnReachDestination;
}