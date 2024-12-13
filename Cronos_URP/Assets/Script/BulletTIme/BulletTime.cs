using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Bullet Time 효과를 관리하는 시스템입니다. 
/// 시간 속도를 조절하여 느리게 또는 정상 속도로 진행되도록 합니다.
/// </summary>
public class BulletTime : MonoBehaviour
{
    [SerializeField]
    private float currentSpeed = 1f;

    public float targetSpeed = 1f;
    public float acceleration = 1f;
    public float deceleration = 1f;


    public UnityEvent OnActive OnNormalrize;

    private static BulletTime _instance;

    public static BulletTime Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BulletTime>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(BulletTime).Name);
                    _instance = obj.AddComponent<BulletTime>();

                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    private void Start()
    {
        currentSpeed += acceleration * 1f;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }
        else
        {
            _instance = this;
        }
    }

    private void LateUpdate()
    {
        if (currentSpeed < targetSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, targetSpeed); // 최대 속도 초과 방지

        }
        else if (currentSpeed > targetSpeed)
        {
            currentSpeed *= (1f - Time.deltaTime * deceleration);
            if (currentSpeed < 0.01f)
            {
                currentSpeed = 0.01f;
            }
            //currentSpeed = Mathf.Max(currentSpeed, maxSpeed); // 최소 속도 미만 방지
        }
    }

    public void DecelerateSpeed()
    {
        targetSpeed = 0.01f;
        OnActive?.Invoke();
    }

    public void SetNormalSpeed()
    {
        targetSpeed = 1f;
        OnNormalrize?.Invoke();
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
