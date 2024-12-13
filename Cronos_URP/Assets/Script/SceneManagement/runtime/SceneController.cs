using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �� ��ȯ �� ���� ����� ����ϴ� �̱��� Ŭ�����Դϴ�.
/// �� �ε�, ��ȯ, ����� �� �ڷ���Ʈ�� ���õ� ����� �����մϴ�.
/// </summary>
public class SceneController : MonoBehaviour
{
    public static SceneController Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            instance = FindObjectOfType<SceneController>();

            if (instance != null)
            {
                return instance;
            }

            Create();

            return instance;
        }
    }

    public static bool Transitioning
    {
        get { return Instance.m_transitioning; }
    }

    protected static SceneController instance;  // �̱��� �ν��Ͻ�
    public SceneTransitionDestination initialSceneTransitionDestination;  // �ʱ� �� ��ȯ ��ġ
    protected Scene m_currentZoneScene;  // ���� �� ����
    protected bool m_transitioning;  // �� ��ȯ �� ����

    /// <summary>
    /// �� ��Ʈ�ѷ� �ν��Ͻ��� �����մϴ�.
    /// </summary>
    public static SceneController Create()
    {
        GameObject sceneControllerGameObject = new GameObject("SceneController");
        instance = sceneControllerGameObject.AddComponent<SceneController>();

        return instance;
    }

    void Awake()
    {
#if UNITY_STANDALONE
        Screen.SetResolution(1920, 1080, true);  // �ػ� ����
#endif

        if (Instance != this)
        {
            Destroy(gameObject);  // �ٸ� �ν��Ͻ��� ������ ��ü�� �ı�
            return;
        }

        DontDestroyOnLoad(gameObject);  // �� ��ȯ �� ��ü�� �ı����� �ʵ��� ����

        if (initialSceneTransitionDestination != null)
        {
            SetEnteringGameObjectLocation(initialSceneTransitionDestination);  // �ʱ� ��ġ ����
            ScreenFader.SetAlpha(1f);
            StartCoroutine(ScreenFader.FadeSceneIn());  // �� ���̵� ��
            initialSceneTransitionDestination.OnReachDestination.Invoke();  // ��ǥ ���� �̺�Ʈ ȣ��
        }
        else
        {
            m_currentZoneScene = SceneManager.GetActiveScene();  // ���� �� ����
        }
    }

    /// <summary>
    /// ���� ���� �ٽ� �����մϴ�.
    /// </summary>
    public static void RestartZone()
    {
        Instance.StartCoroutine(Instance.Transition(Instance.m_currentZoneScene.name));
    }

    /// <summary>
    /// ������ �ð� �� ���� ���� �ٽ� �����մϴ�.
    /// </summary>
    public static void RestartZoneWithDelay(float delay)
    {
        Instance.StartCoroutine(CallWithDelay(delay, RestartZone));
    }

    /// <summary>
    /// �� ������ ��ȯ�մϴ�.
    /// </summary>
    public static void TransitionToScene(TransitionPoint transitionPoint)
    {
        Instance.StartCoroutine(Instance.Transition(transitionPoint.newSceneName));
    }

    /// <summary>
    /// ������ ������ ��ȯ�մϴ�.
    /// </summary>
    public static void TransitionToScene(string newSceneName)
    {
        Instance.StartCoroutine(Instance.Transition(newSceneName));
    }

    /// <summary>
    /// �� ��ȯ�� ó���ϴ� �ڷ�ƾ�Դϴ�.
    /// </summary>
    protected IEnumerator Transition(string newSceneName, TransitionPoint.TransitionType transitionType = TransitionPoint.TransitionType.DifferentZone)
    {
        m_transitioning = true;

        SaveLoadManager.Instance.SaveSceneData();  // �� ������ ����

        yield return StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.Loading));  // �� ���̵� �ƿ�

        yield return SceneLoader.Instance.StartCoroutine(SceneLoader.Instance.LoadSceneCoroutine(newSceneName));  // �� �ε�

        SceneTransitionDestination entrance = GetDestination();  // ��ǥ ��ġ ã��

        SetEnteringGameObjectLocation(entrance);  // ���� ������Ʈ ��ġ ����
        SetupNewScene(transitionType, entrance);  // �� ����

        if (entrance != null)
        {
            entrance.OnReachDestination.Invoke();  // ��ǥ ���� �̺�Ʈ ȣ��
        }

        yield return StartCoroutine(ScreenFader.FadeSceneIn(ScreenFader.FadeType.Loading));  // �� ���̵� ��

        m_transitioning = false;
    }

    /// <summary>
    /// �� ��ȯ ��ǥ�� SceneTransitionDestination�� ã���ϴ�.
    /// </summary>
    protected SceneTransitionDestination GetDestination()
    {
        SceneTransitionDestination entrances = FindObjectOfType<SceneTransitionDestination>();
        if (entrances != null)
        {
            return entrances;
        }

        Debug.LogWarning("SceneTransitionDestination ������Ʈ�� ���� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        return null;
    }

    /// <summary>
    /// ���� ������Ʈ�� ��ġ�� SceneTransitionDestination�� ��ġ�� �����մϴ�.
    /// </summary>
    protected void SetEnteringGameObjectLocation(SceneTransitionDestination entrance)
    {
        if (entrance == null)
        {
            Debug.LogWarning("���� ��ġ(SceneTransitionDestination) �Է��� �������� �ʾҽ��ϴ�.");
            return;
        }
        Transform entranceLocation = entrance.transform;
        Transform enteringTransform = entrance.transitioningGameObject.transform;
        enteringTransform.position = entranceLocation.position;
        enteringTransform.rotation = entranceLocation.rotation;
    }

    /// <summary>
    /// �� ���� �����մϴ�.
    /// </summary>
    protected void SetupNewScene(TransitionPoint.TransitionType transitionType, SceneTransitionDestination entrance)
    {
        if (entrance == null)
        {
            Debug.LogWarning("����� ����(SceneTransitionDestination)�� �������� �ʾҽ��ϴ�.");
            return;
        }

        if (transitionType == TransitionPoint.TransitionType.DifferentZone)
        {
            SetZoneStart(entrance);  // �ٸ� ������ ���, ���� ���� ����
        }
    }

    /// <summary>
    /// ���ο� ������ ������ �����մϴ�.
    /// </summary>
    protected void SetZoneStart(SceneTransitionDestination entrance)
    {
        m_currentZoneScene = entrance.gameObject.scene;
    }

    static IEnumerator CallWithDelay<T>(float delay, Action<T> call, T parameter)
    {
        yield return new WaitForSeconds(delay);
        call(parameter);
    }

    static IEnumerator CallWithDelay(float delay, Action call)
    {
        yield return new WaitForSeconds(delay);
        call();
    }
}
