using Sonity;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

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

    SoundManager soundManager;

    // ����ĳ��Ʈ ����
    float forwardVal = 1.6f;
    float yUpVal = 1.5f;
    public LayerMask groundLayer;
    public float rayMaxDist = 2.0f;

    // �۷ι� ����
    [SerializeField]
    Volume gVolume;
    MotionBlur mBlur;
    ChromaticAberration cAber;
    public float mBlurVal = 0.7f;
    public float cAberVal = 0.7f;
    public float parryTime = 1.0f;
    public float fadeTime = 0.3f;

    // ��ȭ �˱� ����
    [Range(0f, 200f)]
    public float enforceSlashSpeed = 30.0f;
    public float swordWaveSpeed = 20.0f;
    public float swordWaveDistance = 12.0f;
    public GameObject invisibleSlash;
    public bool isGroundEnforced;
    public bool showInvisibleMesh;

    // ��ȭ ���� �����
    public GameObject swordAura;

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
		SceneManager.sceneLoaded += OnSceneLoaded;
    }

	/// <summary>
	/// ����ȯ �ʱ�ȭ�� ���� �Լ�
	/// �ε��ִ� ��ġ���� 
	/// By OHK
	/// </summary>
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
// 		Initialize();
// 		StartCoroutine(LoadEffectCoroutine());
	}

	// �ε��� ����Ʈ�� ���� ������Ʈ �Ҵ�
	void Start()
    {
		Initialize();
		StartCoroutine(LoadEffectCoroutine());
	}

    // ����׸� ���ؼ� �ϴ� ������Ʈ�� �־���Ҵ�
    // ��ȹ �ʿ��� ������ ������ ������ �����Ѵ�
    void Update()
    {
        swordWaveSpeed = enforceSlashSpeed * 2f / 3f;
        swordWaveDistance = enforceSlashSpeed * 2f / 5f;
    }

    private void OnValidate()
    {
        if (invisibleSlash != null)
        {
            ToggleMeshRenderer();
        }
    }

    void Initialize()
    {
        soundManager = SoundManager.Instance;
        player = Player.Instance;
        if(player != null)
        {
            pSword = player.GetComponent<Player>().playerSword;
        }
        gVolume = FindObjectOfType<Volume>();
        if (gVolume != null)
            InitializeVol(gVolume);
        groundLayer = LayerMask.GetMask("Ground");

        if (invisibleSlash != null)
        {
            ToggleMeshRenderer();
        }
        swordAura = GameObject.Find("Com_Ready");
        if (swordAura != null)
            swordAura.SetActive(false);
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

    void ToggleMeshRenderer()
    {
        if (invisibleSlash != null)
        {
            // ������ �ν��Ͻ����� MeshRenderer�� ã�� Ȱ��ȭ/��Ȱ��ȭ
            MeshRenderer meshRenderer = invisibleSlash.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = showInvisibleMesh;
            }
            else
            {
                Debug.LogWarning("MeshRenderer not found on prefab instance.");
            }
        }
    }

    // �÷��̾� ���� ����Ʈ
    public void NormalSlashFX(string fxName)
    {
        // ����Ʈ �̰� �����̼��� Į�� �����̼ǰ� �����.
        // Į�� ����Ʈ�� ������ �ٸ��Ƿ� �̰� ����Ʈ���� ���� �ѹ��� �ʿ���
        // ��ġ�� y ��ǥ�� Į�� ����, �������� �÷��̾� Ʈ����������
        soundManager.PlaySFX("Attack_SE", player.transform);
        GameObject slash = SpawnEffect(fxName, player.transform.position);
        slash.transform.rotation = player.playerSword.transform.rotation;
        slash.transform.Rotate(90f, 180f, 0);
        float newY = player.playerSword.transform.position.y;
        slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
        Destroy(slash, 0.7f);
    }

    // �Ϲ� ������ ����
    public void NormalStrongFX()
    {
        soundManager.PlaySFX("Attack_SE", player.transform);
        GameObject slash = SpawnEffect("Nor_S_Attack", player.transform.position);
        float newY = player.playerSword.transform.position.y;
        slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
        Destroy(slash, 0.7f);
    }

    // ��ȭ ���� Ȱ��ȭ
    public void SwordAuraOn()
    {
        swordAura.SetActive(true);
    }

    public void SwordAuraOff()
    {
        swordAura.SetActive(false);
    }

    // �ϴ� �Ƕ��� ����Ʈ �����
    //public void ComboStrongFX()
    //{
    //    Vector3 impTrans = player.transform.position + player.transform.forward * 1.6f;
    //    GameObject impact = SpawnEffect("Nor04_Attack_Ground", impTrans);
    //    impact.transform.rotation = player.transform.rotation;
    //    impact.transform.Rotate(0, -90f, 0);
    //    Destroy(impact, 2.0f);
    //}

    // ������ ������ �°� ����Ʈ�� ������� ��� �ؾ��ұ�
    public void GroundCheckFX()
    {
        // ���̸� �� ��ġ �÷��̾��� ��ġ + �������� ���� ������ + ���� ����
        // ������ �÷��̾� �����忡�� Į ���� �߽����� ���� �ٲ�
        Vector3 rayTrans = player.transform.position + 
            //player.transform.forward * forwardVal + 
            pSword.transform.up * -1 * forwardVal + 
            new Vector3(0, yUpVal, 0);
        Debug.DrawRay(rayTrans, Vector3.down * rayMaxDist, Color.yellow, 1.0f);
        if (Physics.Raycast(rayTrans, Vector3.down, out RaycastHit hit, rayMaxDist, groundLayer))
        {
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;
            // ProjectOnPlane�� ù��° �Ű����� ���͸� �ι�° �Ű����� �븻�� ������ ���͸� ��ȯ�Ѵ�. 
            Quaternion fxRot = Quaternion.LookRotation(
                //Vector3.ProjectOnPlane(player.transform.forward, hitNormal), hitNormal);
                Vector3.ProjectOnPlane(pSword.transform.up * -1, hitNormal), hitNormal);
            fxRot *= Quaternion.Euler(0, -90f, 0);
            GameObject impact = SpawnEffect("Nor04_Ground", hitPoint, fxRot);
            
            Destroy(impact, 2.0f);
            // �ɷ°���Ǿ��ٸ� �̰͵� ����
            if (isGroundEnforced)
            {
                GameObject cir = SpawnEffect("EnforceGround", hitPoint, fxRot);
                Destroy(cir, 1.0f);
            }
        }
        else
        {
            Debug.Log("no ground impact");
        }
    }

    // �ٴڿ� ��ó�� ����� Nor_Attack_4 ����
    public void GroundScar()
    {
        Vector3 rayTrans = player.transform.position +
            pSword.transform.up * -1 * forwardVal +
            new Vector3(0, yUpVal, 0);
        Debug.DrawRay(rayTrans, Vector3.down * rayMaxDist, Color.yellow, 1.0f);
        if (Physics.Raycast(rayTrans, Vector3.down, out RaycastHit hit, rayMaxDist, groundLayer))
        {
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;
            // ProjectOnPlane�� ù��° �Ű����� ���͸� �ι�° �Ű����� �븻�� ������ ���͸� ��ȯ�Ѵ�. 
            Quaternion fxRot = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(pSword.transform.up * -1, hitNormal), hitNormal);
            fxRot *= Quaternion.Euler(0, -90f, 0);
            GameObject impact = SpawnEffect("Nor04_Ground", hitPoint, fxRot);

            Destroy(impact, 2.0f);
        }
        else
        {
            Debug.Log("no ground impact");
        }
    }

    // �˱� ������
    public void SwordWave()
    {
        GameObject slsh = SpawnEffect("EnforceSwordWave", player.transform.position);
        slsh.transform.position += new Vector3(0, 1f, 0);
        slsh.transform.forward = player.transform.forward;
        slsh.transform.rotation *= Quaternion.Euler(0, 0, -90f);
        var main = slsh.transform.GetChild(1).GetComponent<ParticleSystem>().main;
        main.startSpeed = enforceSlashSpeed;
        Destroy(slsh, 1.0f);

        GameObject invislash = SpawnEffect("InvisibleSlash", player.transform.position);
        invislash.transform.position += new Vector3(0, 1f, 0);
        invislash.transform.forward = player.transform.forward;
        invislash.transform.rotation *= Quaternion.Euler(90f, 0f, 90f);
        StartCoroutine(MoveWave(invislash));
    }

    IEnumerator MoveWave(GameObject proj)
    {
        Vector3 startPos = proj.transform.position;
        Vector3 DestPos = startPos + player.transform.forward * swordWaveDistance;
        //Destroy(proj, 0.7f);
        StartCoroutine(DestroyWave(proj));

        while (proj != null && Vector3.Distance(proj.transform.position, startPos) < swordWaveDistance)
        {
            if (proj == null)
                yield break;

            proj.transform.position = Vector3.MoveTowards(proj.transform.position, DestPos, swordWaveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator DestroyWave(GameObject wav)
    {
        yield return new WaitForSeconds(0.7f);
        if (wav != null)
            Destroy(wav);
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

    public void CreateGuardFX()
    {
        Vector3 grdPos = new Vector3(player.transform.position.x, pSword.transform.position.y, player.transform.position.z);
        GameObject grd = SpawnEffect("GuardFX", grdPos);
        Destroy(grd, 1.0f);
    }

    public void CreateParryFX()
    {
        soundManager.PlaySFX("Parry_SE", player.transform);
        Vector3 parrPos = player.transform.position + new Vector3(0, 1f, 0.25f);
        GameObject parr = SpawnEffect("GuardFlare", parrPos);
        StartCoroutine(FadeOutLights(parr));
        Destroy(parr, 1.0f);
        CreateGuardFX();
        // �۷ι������� ���ٸ� ����
        if (gVolume == null)
            return;
        StartCoroutine(ParryMotionBlurCoroutine(mBlurVal));
        StartCoroutine(ParryCAberrationCoroutine(cAberVal));
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

    IEnumerator FadeOutLights(GameObject flare)
    {
        float elapsedTime = 0.0f;
        Light light = flare.GetComponent<Light>();
        LensFlareComponentSRP lens = flare.GetComponent<LensFlareComponentSRP>();
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            light.intensity = Mathf.Lerp(1.0f, 0f, elapsedTime / fadeTime);
            lens.intensity = Mathf.Lerp(1.0f, 0f, elapsedTime / fadeTime);
            yield return null;
        }

        light.intensity = 0f;
        lens.intensity = 0f;
    }
}
