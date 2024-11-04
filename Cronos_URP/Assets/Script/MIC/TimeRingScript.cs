using System.Collections;
using UnityEngine;

public class TimeRingScript : MonoBehaviour
{
    [SerializeField]
    GameObject timeRing;
    float rotateSpeed = 180.0f;
    private Renderer ringRenderer;
    public Material tex1;
    public Material tex2;
    public Material tex3;
    public Material tex4;
    public Material tex5;

    //[SerializeField]
    float MaxHP;
    Damageable enemy;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == Player.Instance.gameObject)
        {
            Debug.Log("�÷��̾� Ÿ�Ӹ� �۵�");
            MaxHP = Player.Instance.TP;
        }
        else
        {
            Debug.Log("�� Ÿ�Ӹ� �۵�");
            enemy = gameObject.GetComponent<Damageable>();
            MaxHP = enemy.maxHitPoints;
        }
        ringRenderer = timeRing.gameObject.GetComponent<Renderer>();
        StartCoroutine(RotateCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckHealthToSwitchTexture()
    {
        if (enemy == null)
        {
            float curHP = Player.Instance.TP / MaxHP;

            Material[] materials = { tex5, tex4, tex3, tex2, tex1 };  // ü�¿� ���� ���׸��� �迭

            // curHP�� 0~1 ������ ������ �迭 �ε����� ��ȯ (0.8f �̻��� 4, 0.6f �̻��� 3 ��)
            int index = Mathf.Clamp(Mathf.FloorToInt(curHP * 5), 0, 4);

            ringRenderer.material = materials[index];
        }
        else
        {
            float curHP = enemy.currentHitPoints / MaxHP;

            Material[] materials = { tex5, tex4, tex3, tex2, tex1 };  // ü�¿� ���� ���׸��� �迭

            // curHP�� 0~1 ������ ������ �迭 �ε����� ��ȯ (0.8f �̻��� 4, 0.6f �̻��� 3 ��)
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
