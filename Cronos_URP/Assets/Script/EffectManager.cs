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
    GameObject player;

    // ����� ����Ʈ ����Ʈ
    static List<GameObject> effects;


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
        //         UpdateWpos();
        //         UpdateEpos();
    }

    void Initialize()
    {
        
    }

    void LoadEffect()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("FX");
        effects = new List<GameObject>();
        foreach (GameObject effect in prefabs)
        {
            GameObject effectInstance = Instantiate(effect);
            effectInstance.name = effect.name;
            effects.Add(effectInstance);
            effectInstance.SetActive(false);
        }
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

    // �̰� ������ ���ִ� �� �ν��Ͻ��ϰ� ���� ��
    void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
}
