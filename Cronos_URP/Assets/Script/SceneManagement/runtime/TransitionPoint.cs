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


    [Tooltip("�� ��ȯ�� �ɶ� ���� �̵��� ���� ������Ʈ�Դϴ�. (ex. �÷��̾�)")]
    public GameObject transitioningGameObject;
    [Tooltip("��ȯ�� �� ��� ������ �̷������, �ٸ� �������� ��ȯ����, �ƴϸ� �����÷��̰� �ƴ� ������� ��ȯ���� �����մϴ�.")]
    public TransitionType transitionType;
    [SceneName]
    public string newSceneName;
    [Tooltip("�� ������ ��ȯ�Ǵ� ���� ������Ʈ�� �ڷ���Ʈ�� �������Դϴ�.")]
    public TransitionPoint destinationTransform;
    [Tooltip("�� ��ȯ�� ���۵ǵ��� Ʈ�����ؾ� �ϴ� �׸�.")]
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
			else
			{
				Debug.Log("�ȵ��Ծ��!");
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
