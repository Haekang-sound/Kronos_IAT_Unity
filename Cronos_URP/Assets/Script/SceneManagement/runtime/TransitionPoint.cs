using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        DifferentZone,
        DifferentNonGameplayScene,
        SameScene,
    }


    public enum TransitionWhen
    {
        OnTriggerEnter,
        ExternalCall,
    }


    [Tooltip("씬 전환이 될때 같이 이동될 게임 오브젝트입니다. (ex. 플레이어)")]
    public GameObject transitioningGameObject;
    [Tooltip("전환이 이 장면 내에서 이루어질지, 다른 영역으로 전환될지, 아니면 게임플레이가 아닌 장면으로 전환될지 결정합니다.")]
    public TransitionType transitionType;
    [SceneName]
    public string newSceneName;
    [Tooltip("이 씬에서 전환되는 게임 오브젝트가 텔레포트될 목적지입니다.")]
    public TransitionPoint destinationTransform;
    [Tooltip("씬 전환이 시작되도록 트리거해야 하는 항목.")]
    public TransitionWhen transitionWhen;

    bool m_transitioningGameObjectPresent;

    void Start()
    {
        if (transitionWhen == TransitionWhen.ExternalCall)
        {
            m_transitioningGameObjectPresent = true;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == transitioningGameObject)
        {
            m_transitioningGameObjectPresent = true;

            if (ScreenFader.IsFading || SceneController.Transitioning)
            {
                return;
            }

            if (transitionWhen == TransitionWhen.OnTriggerEnter)
            {
                TransitionInternal();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == transitioningGameObject)
        {
            m_transitioningGameObjectPresent = false;
        }
    }

    protected void TransitionInternal()
    {
        if (transitionType == TransitionType.SameScene)
        {
            GameObjectTeleporter.Teleport(transitioningGameObject, destinationTransform.transform);
        }
        else
        {
            SceneController.TransitionToScene(this);
        }
    }

    public void Transition()
    {
        if (m_transitioningGameObjectPresent)
        {
            if (transitionWhen == TransitionWhen.ExternalCall)
            {
                TransitionInternal();
            }
        }
    }

    public void Update()
    {

    }
}
