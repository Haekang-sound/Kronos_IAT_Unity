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
    [Tooltip("8���� ������ ����ũ �����Դϴ�.")]
    [Range(0, 10)]
    public float rayStrength = 1.0f;
    [Tooltip("��Ȧ ������ ����ũ �����Դϴ�. 8���� ������ ���� ȣ���մϴ�.")]
    [Range(0, 10)]
    public float blackHoleStrength = 1.0f;
    [Tooltip("��ħ�� ����ũ �����Դϴ�.")]
    [Range(0, 10)]
    public float handsStrength = 1.0f;

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
            Debug.Log("���: InpulseCam Ŭ�������� CinemachineImpulseSource �� ����");
        }
    }
}
