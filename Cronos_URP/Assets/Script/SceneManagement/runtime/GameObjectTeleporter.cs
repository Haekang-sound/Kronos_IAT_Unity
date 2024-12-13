using System.Collections;
using UnityEngine;

/// <summary>
/// ���� ������Ʈ�� �ڷ���Ʈ ����� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class GameObjectTeleporter : MonoBehaviour
{
    public static GameObjectTeleporter Instance
    {
        get
        {
            if (instance != null)
                return instance;

            instance = FindObjectOfType<GameObjectTeleporter>();

            if (instance != null)
                return instance;

            // GameObjectTeleporter ��ü�� ������ ���� ����
            GameObject gameObjectTeleporter = new GameObject("GameObjectTeleporter");
            instance = gameObjectTeleporter.AddComponent<GameObjectTeleporter>();

            return instance;
        }
    }

    public static bool Transitioning
    {
        get { return Instance.m_transitioning; }
    }

    protected static GameObjectTeleporter instance;  // �̱��� �ν��Ͻ�
    protected bool m_transitioning;  // �ڷ���Ʈ ���� �� ����

    void Awake()
    {
        // �̹� �̱��� �ν��Ͻ��� ������ ��ü�� �ı��ϰ�, �׷��� ������ ���� ��ü�� ����
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);  // �� ��ȯ �� ��ü�� �ı����� �ʵ��� ����
    }

    /// <summary>
    /// ������ TransitionPoint�� �ڷ���Ʈ
    /// </summary>
    public static void Teleport(TransitionPoint transitionPoint)
    {
        Transform destinationTransform = Instance.GetDestination().transform;
        Instance.StartCoroutine(Instance.Transition(transitionPoint.transitioningGameObject, true, destinationTransform.position, true));
    }

    /// <summary>
    /// ���� ������Ʈ�� ������ Transform ��ġ�� �ڷ���Ʈ
    /// </summary>
    public static void Teleport(GameObject transitioningGameObject, Transform destination)
    {
        Instance.StartCoroutine(Instance.Transition(transitioningGameObject, false, destination.position, false));
    }

    /// <summary>
    /// ���� ������Ʈ�� ������ ��ġ�� �ڷ���Ʈ
    /// </summary>
    public static void Teleport(GameObject transitioningGameObject, Vector3 destinationPosition)
    {
        Instance.StartCoroutine(Instance.Transition(transitioningGameObject, false, destinationPosition, false));
    }

    /// <summary>
    /// �ڷ���Ʈ ������ ó���ϴ� �ڷ�ƾ
    /// </summary>
    protected IEnumerator Transition(GameObject transitioningGameObject, bool releaseControl, Vector3 destinationPosition, bool fade)
    {
        m_transitioning = true;

        if (fade)
        {
            yield return StartCoroutine(ScreenFader.FadeSceneOut());  // �� ���̵� �ƿ�
        }

        transitioningGameObject.transform.position = destinationPosition;  // ��ġ �̵�

        if (fade)
        {
            yield return StartCoroutine(ScreenFader.FadeSceneIn());  // �� ���̵� ��
        }

        m_transitioning = false;
    }

    /// <summary>
    /// �ڷ���Ʈ ��ǥ ��ġ�� SceneTransitionDestination ��ü�� ã��
    /// </summary>
    protected SceneTransitionDestination GetDestination()
    {
        SceneTransitionDestination entrance = FindObjectOfType<SceneTransitionDestination>();
        if (entrance != null)
        {
            return entrance;
        }

        Debug.LogWarning("SceneTransitionDestination �� ã�� �� �����ϴ�.");
        return null;
    }
}
