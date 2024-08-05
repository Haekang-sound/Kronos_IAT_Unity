using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    
    // �÷��̾� ����
    [SerializeField]
    Player player;
    GameObject pSword;

    // �۷ι� ����
    [SerializeField]
    Volume gVolume;
    MotionBlur mBlur;
    ChromaticAberration cAber;
    public float mBlurVal = 0.7f;
    public float cAberVal = 0.7f;
    public float parryTime = 1.0f;

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
            Debug.Log(gameObject.name + " destroyed");
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Effect Manager called on " + gameObject.name);
        }
    }

    // �ε��� ����Ʈ�� ���� ������Ʈ �Ҵ�
    void Start()
    {
        Initialize();
        StartCoroutine(LoadEffectCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize()
    {
        player = Player.Instance;
        pSword = player.GetComponent<Player>().playerSword;
        gVolume = FindObjectOfType<Volume>();
        if (gVolume != null)
            InitializeVol(gVolume);
    }

    void InitializeVol(Volume vol)
    {
        vol.profile.TryGet(out mBlur);
        vol.profile.TryGet(out cAber);

        if(mBlur != null)
        {
            mBlur.active = true;
            mBlur.intensity.value = 0.0f;
        }

        if(cAber != null)
        {
            cAber.active = true;
            cAber.intensity.value = 0.0f;
        }
    }

    IEnumerator LoadEffectCoroutine()
    {
        effectArray = Resources.LoadAll<GameObject>("Prefabs/FX");
        foreach (GameObject effect in effectArray)
        {
            GameObject effectInstance = Instantiate(effect);
            effectInstance.name = effect.name;
            effects.Add(effectInstance);
            effectInstance.SetActive(false);

            yield return null;
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
    // �ε� �̷��� �ʿ��ϳ�?
    //void TurnOffObject(GameObject obj)
    //{
    //    obj.SetActive(false);
    //}

    //void TurnOnObject(GameObject obj)
    //{
    //    obj.SetActive(true);
    //}

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

    public void CreateParryFX()
    {
        Vector3 parrPos = new Vector3(player.transform.position.x, pSword.transform.position.y, player.transform.position.z);
        GameObject parr = SpawnEffect("ParryY", parrPos);
        Destroy(parr, 1.5f);
        StartCoroutine(ParryMotionBlurCoroutine(mBlurVal));
        StartCoroutine(ParryCAberrationCoroutine(cAberVal));
    }

    public void CreateGuardFX()
    {
        Vector3 grdPos = new Vector3(player.transform.position.x, pSword.transform.position.y, player.transform.position.z);
        GameObject grd = SpawnEffect("GuardFX", grdPos);
        Destroy(grd, 1.0f);
    }

    IEnumerator ParryMotionBlurCoroutine(float val)
    {
        mBlur.intensity.value = val;
        float elapsedTime = 0.0f;
        while (elapsedTime < parryTime)
        {
            mBlur.intensity.value = Mathf.Lerp(val, 0f, elapsedTime / parryTime);
            elapsedTime += Time.deltaTime;
            Debug.Log("blur intensity : " + mBlur.intensity.value);
            yield return null;
        }

        mBlur.intensity.value = 0f;
    }

    IEnumerator ParryCAberrationCoroutine(float val)
    {
        cAber.intensity.value = val;
        float elapsedTime = 0.0f;
        while (elapsedTime < parryTime)
        {
            cAber.intensity.value = Mathf.Lerp(val, 0, elapsedTime / parryTime);
            elapsedTime += Time.deltaTime;
            Debug.Log("aberration intensity : " + cAber.intensity.value);
            yield return null;
        }

        cAber.intensity.value = 0f;
    }
}
