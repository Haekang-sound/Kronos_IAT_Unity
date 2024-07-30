using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // �̱���
    private static EffectManager instance;
    // Get�ϴ� ������Ƽ
    public static EffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectManager>();
                if (instance == null)
                {
                    GameObject effectManager = new GameObject(typeof(EffectManager).Name);
                    instance = effectManager.AddComponent<EffectManager>();

                    DontDestroyOnLoad(effectManager);
                }
            }
            return instance;
        }
    }

    [SerializeField]
    Player player;

    public GameObject parryFX;

    // ����� ����Ʈ ����Ʈ
    static List<GameObject> effects = new List<GameObject>();
    GameObject[] effectArray;

    // ����Ʈ�� �ε��ϴ� �ܰ�
    protected void Awake()
    {
        // �̹� �ν��Ͻ��� �����Ѵٸ�
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Debug.Log("Effect Manager Ȱ��ȭ");
        LoadEffect();
    }

    // �ε��� ����Ʈ�� ���� ������Ʈ �Ҵ�
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        parryFX.transform.position = player.GetComponent<Player>().playerSword.transform.position;
    }

    void Initialize()
    {
        player = Player.Instance;
        parryFX = FindName("ParryGreen");
        parryFX.SetActive(false);
    }

    void LoadEffect()
    {
        effectArray = Resources.LoadAll<GameObject>("Prefabs/FX");
        foreach (GameObject effect in effectArray)
        {
            GameObject effectInstance = Instantiate(effect);
            effectInstance.name = effect.name;
            effects.Add(effectInstance);
            effectInstance.SetActive(false);
        }
    }

    public GameObject SpawnEffect(string name, Vector3 pos)
    {
        foreach (GameObject effect in effectArray)
        {
            if (effect.name == name)
            {
                GameObject instance = Instantiate(effect);
                instance.transform.position = pos;
                return instance;
            }
        }

        return null;
    }

    GameObject FindName(string name)
    {
        foreach (GameObject effect in effects)
        {
            if (effect.name == name)
                return effect;
        }
        return null;
    }

    // �θ� ������Ʈ���� �̸��� ���� �ڽ� ������Ʈ�� ����
    // ����Ʈ�� ���� �ڽ� ������Ʈ ��ġ ã�� �� ����ϴ� ��
    GameObject FindChild(GameObject parent, string name)
    {
        GameObject result = GameObject.Find(name);
        if (result != null)
            return result;
        foreach (GameObject gameObject in parent.GetComponentsInChildren<GameObject>())
        {
            result = FindChild(gameObject, name);
            if (result != null)
                return result;
        }
        return null;
    }

    // ������Ʈ�� SetActive�� false�� �ϴ� ��
    void TurnOffObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    void TurnOnObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    // ����Ʈ�Ŵ����� ��� �ִ°� ���� �� ������
    public void CreateHitFX(Damageable.DamageMessage dmgMsg, Transform targetTrans)
    {
        // �������
        GameObject frag = SpawnEffect("FragFX", targetTrans.position);
        frag.transform.LookAt(dmgMsg.damageSource);
        frag.transform.Rotate(-15f, 0, 0);
        Destroy(frag, 2.0f);

        // �ǰ�����Ʈ
        Vector3 newPos = new Vector3(targetTrans.position.x - dmgMsg.direction.x, dmgMsg.damageSource.y, targetTrans.position.z);
        GameObject slashed = SpawnEffect("UpSlash", newPos);
        slashed.transform.forward = Camera.main.transform.forward;
        Destroy(slashed, 1.0f);
    }
}
