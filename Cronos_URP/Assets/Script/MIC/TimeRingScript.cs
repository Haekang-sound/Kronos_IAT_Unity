using System.Collections;
using UnityEngine;


/// <summary>
/// 타임 링 클래스
/// 시작하자마자 회전 코루틴을 실행해서 계속해서 회전한다
/// 부모 오브젝트의 HP에 따라서 다른 마테리얼을 가져온다
/// </summary>
public class TimeRingScript : MonoBehaviour
{
    [SerializeField]
    private GameObject timeRing;
    float rotateSpeed = 180.0f;
    private Renderer ringRenderer;
    public Material tex1;
    public Material tex2;
    public Material tex3;
    public Material tex4;
    public Material tex5;

    private float maxHP;
    private Damageable enemy;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == Player.Instance.gameObject)
        {
            Debug.Log("플레이어 타임링 작동");
            maxHP = Player.Instance.TP;
        }
        else
        {
            Debug.Log("적 타임링 작동");
            enemy = gameObject.GetComponent<Damageable>();
            maxHP = enemy.maxHitPoints;
        }
        ringRenderer = timeRing.gameObject.GetComponent<Renderer>();
        StartCoroutine(RotateCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 계속 플레이어의 HP를 체크하고, 특정 범위를 넘어갈 때 마테리얼을 바꾸는게 좋겠다
    void CheckHealthToSwitchTexture()
    {
        if (enemy == null)
        {
            float curHP = Player.Instance.TP / maxHP;

            Material[] materials = { tex5, tex4, tex3, tex2, tex1 };  // 체력에 따른 마테리얼 배열

            // curHP를 0~1 범위로 나누고 배열 인덱스로 변환 (0.8f 이상은 4, 0.6f 이상은 3 등)
            int index = Mathf.Clamp(Mathf.FloorToInt(curHP * 5), 0, 4);

            ringRenderer.material = materials[index];
        }
        else
        {
            float curHP = enemy.currentHitPoints / maxHP;

            Material[] materials = { tex5, tex4, tex3, tex2, tex1 };  // 체력에 따른 마테리얼 배열

            // curHP를 0~1 범위로 나누고 배열 인덱스로 변환 (0.8f 이상은 4, 0.6f 이상은 3 등)
            int index = Mathf.Clamp(Mathf.FloorToInt(curHP * 5), 0, 4);

            ringRenderer.material = materials[index];
        }
    }

    public IEnumerator RotateCoroutine()
    {
        while (true)
        {
            CheckHealthToSwitchTexture();
            timeRing.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
