using UnityEngine;
using Cinemachine;

public class ImpulseCam : MonoBehaviour
{
    private static ImpulseCam instance;
    public static ImpulseCam Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ImpulseCam>();
                if (instance == null)
                {
                    GameObject ic = new GameObject(typeof(ImpulseCam).Name);
                    instance = ic.AddComponent<ImpulseCam>();

                    DontDestroyOnLoad(ic);
                }
            }
            return instance;
        }
    }

    CinemachineImpulseSource impulse;
    [Range(0, 10)]
    public float shakeStrength = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        impulse = transform.GetComponent<CinemachineImpulseSource>();
    }

    public void Shake()
    {
        impulse.GenerateImpulse(1f);
    }

    public void Shake(float pow)
    {
        if (impulse != null)
        {
            impulse.GenerateImpulse(pow);
        }
        else
        {
            Debug.Log("경고: InpulseCam 클래스에서 CinemachineImpulseSource 이 없음");
        }
    }
}
