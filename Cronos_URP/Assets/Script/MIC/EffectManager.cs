using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectManager : MonoBehaviour
{
    // 싱글턴
    private static EffectManager instance;
    // Get하는 프로퍼티
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
    
    // 플레이어 관련
    [SerializeField]
    Player player;
    GameObject pSword;

    SoundManager soundManager;

    // 레이캐스트 관련
    float forwardVal = 1.6f;
    float yUpVal = 1.5f;
    public LayerMask groundLayer;
    public float rayMaxDist = 2.0f;

    // 글로벌 볼륨
    [SerializeField]
    Volume gVolume;
    MotionBlur mBlur;
    ChromaticAberration cAber;
    public float mBlurVal = 0.7f;
    public float cAberVal = 0.7f;
    public float parryTime = 1.0f;

    // 사용할 이펙트 리스트
    static List<GameObject> effects = new List<GameObject>();
    GameObject[] effectArray;

    // 이펙트를 로드하는 단계
    protected void Awake()
    {
        // 이미 인스턴스가 존재한다면
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

    // 로드한 이펙트에 게임 오브젝트 할당
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
        soundManager = SoundManager.Instance;
        player = Player.Instance;
        pSword = player.GetComponent<Player>().playerSword;
        gVolume = FindObjectOfType<Volume>();
        if (gVolume != null)
            InitializeVol(gVolume);
        groundLayer = LayerMask.GetMask("Ground");
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

    public GameObject SpawnEffect(string name, Vector3 pos, Quaternion rot)
    {
        foreach (GameObject effect in effectArray)
        {
            if (effect.name == name)
            {
                GameObject instance = Instantiate(effect);
                instance.transform.position = pos;
                instance.transform.rotation = rot;
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

    // 부모 오브젝트에서 이름을 가진 자식 오브젝트를 리턴
    // 이펙트가 나올 자식 오브젝트 위치 찾는 데 사용하는 중
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

    // 오브젝트의 SetActive를 false로 하는 것
    // 인데 이런게 필요하냐?
    //void TurnOffObject(GameObject obj)
    //{
    //    obj.SetActive(false);
    //}

    //void TurnOnObject(GameObject obj)
    //{
    //    obj.SetActive(true);
    //}

    // 플레이어 관련 이펙트
    public void NormalSlashFX(string fxName)
    {
        // 이펙트 뽑고 로테이션을 칼의 로테이션과 맞춘다.
        // 칼과 이펙트의 기준이 다르므로 이건 이펙트마다 매직 넘버가 필요함
        // 위치는 y 좌표만 칼과 같게, 나머지는 플레이어 트랜스폼에서
        soundManager.PlaySFX("Attack_SE", player.transform);
        GameObject slash = SpawnEffect(fxName, player.transform.position);
        slash.transform.rotation = player.playerSword.transform.rotation;
        slash.transform.Rotate(90f, 180f, 0);
        float newY = player.playerSword.transform.position.y;
        slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
        Destroy(slash, 0.7f);
    }

    // 일반 강공격 스핀
    public void NormalStrongFX()
    {
        soundManager.PlaySFX("Attack_SE", player.transform);
        GameObject slash = SpawnEffect("Nor_S_Attack", player.transform.position);
        float newY = player.playerSword.transform.position.y;
        slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
        Destroy(slash, 0.7f);
    }

    // 일단 맨땅에 이펙트 만들기
    public void ComboStrongFX()
    {
        Vector3 impTrans = player.transform.position + player.transform.forward * 1.6f;
        GameObject impact = SpawnEffect("Nor04_Attack_Ground", impTrans);
        impact.transform.rotation = player.transform.rotation;
        impact.transform.Rotate(0, -90f, 0);
        Destroy(impact, 2.0f);
    }

    // 지면의 각도에 맞게 이펙트를 남기려면 어떻게 해야할까
    public void GroundCheckFX()
    {
        // 레이를 쏠 위치 플레이어의 위치 + 정면으로 조금 앞으로 + 조금 위로
        Vector3 rayTrans = player.transform.position + 
            player.transform.forward * forwardVal + 
            new Vector3(0, yUpVal, 0);
        Debug.DrawRay(rayTrans, Vector3.down * rayMaxDist, Color.yellow, 1.0f);
        if (Physics.Raycast(rayTrans, Vector3.down, out RaycastHit hit, rayMaxDist, groundLayer))
        {
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;
            // ProjectOnPlane은 첫번째 매개변수 벡터를 두번째 매개변수 노말에 투영된 벡터를 반환한다. 
            Quaternion fxRot = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(player.transform.forward, hitNormal), hitNormal);
            fxRot *= Quaternion.Euler(0, -90f, 0);
            GameObject impact = SpawnEffect("Nor04_Ground", hitPoint, fxRot);
            
            Destroy(impact, 2.0f);
        }
        else
        {
            Debug.Log("no ground impact");
        }
    }

    // 능력개방이 되었다면 이거랑 같이
    public void ComboImpactNSlash()
    {
        GameObject cir = SpawnEffect("ImpactCircle", player.transform.position);
        Destroy(cir, 1.0f);
        //GameObject slsh = SpawnEffect("ComboSlash", player.transform.position);
        //slsh.transform.position = new Vector3(
        //    player.transform.position.x,
        //    player.transform.position.y + 1.0f,
        //    player.transform.position.z);
        //Destroy(slsh, 1.0f);
    }

    // 이펙트매니저가 들고 있는게 나을 것 같은데
    public void CreateHitFX(Damageable.DamageMessage dmgMsg, Transform targetTrans)
    {
        // 파편만들기
        GameObject frag = SpawnEffect("FragFX", targetTrans.position);
        frag.transform.LookAt(dmgMsg.damageSource);
        frag.transform.Rotate(-15f, 0, 0);
        Destroy(frag, 2.0f);

        // 피격이펙트
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
