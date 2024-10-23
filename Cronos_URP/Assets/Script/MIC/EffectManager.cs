using Sonity;
using System.Collections;
using System.Collections.Generic;
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
	public GameObject pSword;

	SoundManager soundManager;

	// ����ĳ��Ʈ ����
	float forwardVal = 1.6f;
	float yUpVal = 1.5f;
	public LayerMask groundLayer;
	public float rayMaxDist = 2.0f;

	// �۷ι� ����
	[SerializeField]
	Volume eVolume;
	MotionBlur mBlur;
	ChromaticAberration cAber;
	DepthOfField dOF;
	public float mBlurVal = 1.0f;
	public float cAberVal = 0.7f;
	public float dofFDistance = 2.0f;
	public float dofFLength = 100.0f;
	public float dofAperture = 6.0f;

	public float parryTime = 1.0f;
	public float fadeTime = 0.3f;

	// ĳ���� ��Ÿ ������ ���߱� ���ؼ�
	Vector3 swordMagicOffest = new Vector3(90, 180, 0);
	Vector3 enemyMagicOffset = new Vector3(0, 180, 0);

	// ��ȭ �˱� ����
	public bool isSwordWave;
	[Range(0f, 200f)]
	public float enforceSlashSpeed = 30.0f;
	public float swordWaveSpeed = 20.0f;
	public float swordWaveDistance = 12.0f;
	public GameObject invisibleSlash;
	public bool isGroundEnforced;
	public bool showInvisibleMesh;

	// ��ȭ ���� �����
	public GameObject swordAura;

	// ��ƼŬ�� ���Ƶ��̴� �ڽ�
	public GameObject absorbBox;

	// ���� ����Ʈ ����
	public float bossBeamDistance = 4.0f;
	public float bossBeamTerm = 0.2f;
	public float bossBeamDupeTime = 8.0f;

	public float bossMoonHeight = 4.0f;
	public float bossMoonDistance = 5.0f;

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
		//Initialize();
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
        //swordWaveSpeed = enforceSlashSpeed * 2f / 3f;
        //swordWaveDistance = enforceSlashSpeed * 2f / 5f;

        //���� ����Ʈ ����� ������
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    StartCoroutine(BossEightBeamCoroutine(player.transform));
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //    BossFireShoot(player.transform);
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //    BossFiveSpear(player.transform);
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //    BossMoon(player.transform);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            CreateAbsorbFX(player.transform, 12);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            SpeedLine();
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
        if (eVolume == null)
        {
            // ����� ũ������ �˺��� ������ ���ٴµ�
            GameObject eVol = GameObject.Find("Effect Volume");
            eVolume = eVol.GetComponent<Volume>();
        }

        if (eVolume != null)
            InitializeVol(eVolume);
        groundLayer = LayerMask.GetMask("Ground");

        if (invisibleSlash != null)
        {
            ToggleMeshRenderer();
        }
        if (swordAura == null)
            swordAura = GameObject.Find("Com_Ready");

        if (swordAura != null)
            swordAura.SetActive(false);

        if (absorbBox == null)
            absorbBox = GameObject.Find("AbsorbBox");
    }

    void InitializeVol(Volume vol)
    {
        vol.profile.TryGet(out mBlur);
        vol.profile.TryGet(out cAber);
        vol.profile.TryGet(out dOF);

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

        if (dOF != null)
        {
            dOF.active = true;
            dOF.focusDistance.value = 0.0f;
            dOF.focalLength.value = 0.0f;
            dOF.aperture.value = 0.0f;
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

    public GameObject SpawnEffect(string name, Vector3 pos, float value)
    {
        foreach (GameObject effect in effectArray)
        {
            if (effect.name == name)
            {
                GameObject instance = Instantiate(effect);
                instance.transform.position = pos;
                if (instance.GetComponent<AheadToHUD>() != null)
                instance.GetComponent<AheadToHUD>().BurstNumber = value;

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
        slash.transform.rotation = player.playerSword.transform.rotation * Quaternion.Euler(swordMagicOffest);
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

    // ������ ����
    public void DodgeAttack()
    {
        soundManager.PlaySFX("Attack_SE", player.transform);
        GameObject dttack = SpawnEffect("DodgeAttack", player.transform.position);
        float newY = player.playerSword.transform.position.y;
        dttack.transform.position = new Vector3(dttack.transform.position.x, newY, dttack.transform.position.z);
        dttack.transform.forward = player.transform.forward;
        Destroy(dttack, 4.0f);
    }

    // ����Ƽ ���� 1
    public void AbilitySlash()
    {
        soundManager.PlaySFX("Attack_SE", player.transform);
        GameObject aSlash = SpawnEffect("AbilitySlash", player.transform.position);
        aSlash.transform.rotation = player.playerSword.transform.rotation;
        float newY = player.playerSword.transform.position.y;
        aSlash.transform.position = new Vector3(aSlash.transform.position.x, newY, aSlash.transform.position.z);
        Destroy(aSlash, 2.0f);
    }

    // ���� ���� �⺻����
    public void EnemySlash(Transform enemy)
    {
        // ����Ʈ �̰� �����̼��� Į�� �����̼ǰ� �����.
        // Į�� ����Ʈ�� ������ �ٸ��Ƿ� �̰� ����Ʈ���� ���� �ѹ��� �ʿ���
        // ��ġ�� y ��ǥ�� Į�� ����, �������� �÷��̾� Ʈ����������
        GameObject slash = SpawnEffect("EnemyAttack", enemy.position);
        slash.transform.rotation = enemy.gameObject.GetComponent<ATypeEnemyBehavior>().
            enemySword.transform.rotation * Quaternion.Euler(enemyMagicOffset);
        float newY = enemy.gameObject.GetComponent<ATypeEnemyBehavior>().
            enemySword.transform.position.y;
        slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
        Destroy(slash, 0.7f);
    }

    // �� ���� ��������
    public void EnemyCharge(Transform enemy)
    {
        GameObject charge = SpawnEffect("EnemyCharge", enemy.position);
        charge.transform.position += new Vector3(0, 1f, 0);
        charge.transform.forward = enemy.forward;
        Destroy(charge, 2.0f);
    }

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
    // �̾��µ� ���� �ɷ°��濡���� �Ἥ ���� �޾ƾߵ�
    public void GroundScar(string name)
    {
        Debug.Log("�� üũ ����");
        Vector3 rayTrans = player.transform.position +
            pSword.transform.up * -1 * forwardVal +
            new Vector3(0, yUpVal, 0);
        Debug.DrawRay(rayTrans, Vector3.down * rayMaxDist, Color.yellow, 1.0f);
        if (Physics.Raycast(rayTrans, Vector3.down, out RaycastHit hit, rayMaxDist, groundLayer))
        {
            Debug.Log("������ ���� �ִ�");
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;
            // ProjectOnPlane�� ù��° �Ű����� ���͸� �ι�° �Ű����� �븻�� ������ ���͸� ��ȯ�Ѵ�. 
            Quaternion fxRot = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(pSword.transform.up * -1, hitNormal), hitNormal);
            fxRot *= Quaternion.Euler(0, -90f, 0);
            GameObject impact = SpawnEffect(name, hitPoint, fxRot);

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
        if (isSwordWave)
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
            StartCoroutine(MoveWaveCoroutine(invislash));
        }
    }

    IEnumerator MoveWaveCoroutine(GameObject proj)
    {
        Vector3 startPos = proj.transform.position;
        Vector3 DestPos = startPos + player.transform.forward * swordWaveDistance;
        //Destroy(proj, 0.7f);
        StartCoroutine(DestroyWaveCoroutine(proj));

        while (proj != null && Vector3.Distance(proj.transform.position, startPos) < swordWaveDistance)
        {
            if (proj == null)
                yield break;

            proj.transform.position = Vector3.MoveTowards(proj.transform.position, DestPos, swordWaveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator DestroyWaveCoroutine(GameObject wav)
    {
        yield return new WaitForSeconds(0.7f);
        if (wav != null)
            Destroy(wav);
    }

    // ����Ʈ�Ŵ����� ��� �ִ°� ���� �� ������
    public void CreateHitFX(Damageable.DamageMessage dmgMsg, Transform targetTrans)
    {
        // ��������
        GameObject frag = SpawnEffect("FragFX", targetTrans.position);
        frag.transform.LookAt(dmgMsg.damageSource);
        frag.transform.Rotate(-15f, 0, 0);
        Destroy(frag, 2.0f);

        // �ǰ�����Ʈ
        Vector3 newPos = new Vector3(targetTrans.position.x - dmgMsg.direction.x, dmgMsg.damageSource.y, targetTrans.position.z);
        GameObject slashed = SpawnEffect("Nor_Damage", newPos);
        slashed.transform.forward = Camera.main.transform.forward;
        slashed.transform.position += new Vector3(0, 1, 0);
        slashed.transform.rotation = player.playerSword.transform.rotation *
            Quaternion.Euler(new Vector3(90f, 0, 0));
        Destroy(slashed, 1.0f);
    }

    public void CreateAbsorbFX(Transform trans, float burstNum)
    {
        // 1�� �ڿ� hud�� �����ϴ� ��ƼŬ �����
        GameObject ab = SpawnEffect("AbsorbFX", trans.position + new Vector3(0, 2, 0), burstNum);
        Destroy(ab, 3.0f);
    }

    public void CreateGuardFX()
    {
        Vector3 grdPos = new Vector3(player.transform.position.x, pSword.transform.position.y, player.transform.position.z);
        GameObject grd = SpawnEffect("GuardFX", grdPos);
        Destroy(grd, 1.0f);
    }

    public void CreateParryFX()
    {
        soundManager.PlaySFX("Parry_Sound_SE", player.transform);
        Vector3 parrPos = player.transform.position + new Vector3(0, 1f, 0.25f);
        GameObject parr = SpawnEffect("GuardFlare", parrPos);
        StartCoroutine(FadeOutLightsCoroutine(parr));
        Destroy(parr, 1.0f);
        CreateGuardFX();
        // �۷ι������� ���ٸ� ����
        if (eVolume == null)
            return;
        StartCoroutine(ParryMotionBlurCoroutine(mBlurVal));
        StartCoroutine(ParryCAberrationCoroutine(cAberVal));
        StartCoroutine(ParryDepthOfFieldCoroutine());
        StartCoroutine(ParryTime(0.2f));
        
    }

    // ���� 8���� ��
    public IEnumerator BossEightBeamCoroutine(Transform bossTrans)
    {
        for (int i = 0; i < bossBeamDupeTime; i++)
        {
            soundManager.PlaySFX("Beam_SE", player.transform);

            for (int j = 0; j < 8; j++)
            {
                GameObject beam = SpawnEffect("BossFX_1Beam", bossTrans.position);
                beam.transform.Rotate(0, 45.0f * j, 0);

                GameObject beamBase = beam.transform.GetChild(0).gameObject;
                beamBase.transform.position += beamBase.transform.right * (bossBeamDistance * i);

                Destroy(beam, 0.6f);
            }
            yield return new WaitForSeconds(bossBeamTerm);
        }
    }

    // ���� �� ���� �����
    public void BossFireShoot(Transform bosstrans)
    {
        GameObject fire = SpawnEffect("BossFX_FireProjectile", bosstrans.position);
        fire.transform.forward = bosstrans.transform.forward;
        fire.transform.position += new Vector3(0, 1.0f, 0);
    }

    // ���� â 5�� ���
    public void BossFiveSpear(Transform bossTrans)
    {
        Vector3 forward = bossTrans.forward;
        Vector3 newOffset = new Vector3(0, 3.5f, 0);
        Vector3 newPos = bossTrans.TransformPoint(newOffset);
        GameObject spears = SpawnEffect("BossFX_Spears", newPos);
        spears.transform.forward = forward;
        Destroy(spears, 15.0f);
    }

    // â ���� ����Ʈ
    public void SpearImpact(Vector3 pos)
    {
        GameObject imp = SpawnEffect("BossFX_SpearImpact", pos);
        Destroy(imp, 3.0f);
    }

    // ���� ���� �����
    public void BossMoon(Transform bossTrans)
    {
        List<int> moonNums = new List<int>();
        moonNums = FisherYatesShuffles(8, 5);
        Vector3 newOffset = new Vector3(0, bossMoonHeight, 0);
        Vector3 newPos = bossTrans.TransformPoint(newOffset);

        for (int i = 0; i < moonNums.Count; i++)
        {
            GameObject moon = SpawnEffect("BossFX_BlackHole", newPos);
            moon.transform.Rotate(0, 45.0f * moonNums[i], 0);
            moon.transform.position += moon.transform.forward * bossMoonDistance;
        }
    }

    // ���߼� �״ٰ� ���
    IEnumerator SpeedLineCoroutine()
    {
        UI_TPCPHUD.GetInstance().speedLineUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        UI_TPCPHUD.GetInstance().speedLineUI.SetActive(false);
    }

    public void SpeedLine()
    {
        StartCoroutine(SpeedLineCoroutine());
    }

    // ���� ���� ���� ��¦
    


    // �и����� �� ��� ���
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

    // �и����� �� ũ�θ�ƽ �ֹ����̼�
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

    // �и����� �� ���� ���� �ʵ� �ð��� �ٸ� ȿ������ ���� ���� ������
    IEnumerator ParryDepthOfFieldCoroutine()
    {
        dOF.focusDistance.value = dofFDistance;
        dOF.focalLength.value = dofFLength;
        dOF.aperture.value = dofAperture;
        float elapsedTime = 0.0f;
        float dofTime = 0.3f;
        while (elapsedTime < parryTime)
        {
            elapsedTime += Time.deltaTime;
            dOF.focusDistance.value = Mathf.Lerp(dofFDistance, 0.1f, elapsedTime / dofTime);
            dOF.focalLength.value = Mathf.Lerp(dofFLength, 1f, elapsedTime / dofTime);
            dOF.aperture.value = Mathf.Lerp(dofAperture, 1f, elapsedTime / dofTime);
            yield return null;
        }

        dOF.focusDistance.value = 0.1f;
        dOF.focalLength.value = 1f;
        dOF.aperture.value = 1f;
    }

    // �и����� �� �ð� ���̱�
    IEnumerator ParryTime(float val)
    {
        // val ��ŭ Ÿ�ӽ����� �ٿ��� ������� ������
        Time.timeScale = val;
        float elapsedTime = 0.0f;
        // n�ʿ� ���ļ� ���ư���
        float backTime = 0.6f;
        while (elapsedTime < backTime)
        {
            elapsedTime += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(val, 1f, elapsedTime / backTime);
            yield return null;
        }
    }

    IEnumerator FadeOutLightsCoroutine(GameObject flare)
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


    // elements ���� ���Ҹ� ������ result ���� ���� ���� �տ��� �̴�
    // �Ǽ�-������ �˰����
    public List<int> FisherYatesShuffles(int elements, int result)
    {
        // 0���� elements���� ���ڸ� ����Ʈ�� ����
        List<int> numbers = new List<int>();
        for (int i = 0; i < elements; i++)
        {
            numbers.Add(i);
        }

        // ���� ��ü ����
        System.Random rand = new System.Random();

        // �Ǽ�-������ �˰�������� ����Ʈ ����
        for (int i = numbers.Count - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1); // 0���� i������ �ε��� �� �ϳ��� ����
            // numbers[i]�� numbers[j]�� swap
            int temp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }

        // ��� ���
        Debug.Log("���� �迭: " + string.Join(", ", numbers));

        List<int> numbers2 = new List<int>();
        for (int i = 0; i < result; i++)
        {
            numbers2.Add(numbers[i]);
        }

        Debug.Log("number2 �迭: " + string.Join(", ", numbers2));

        return numbers2;
    }
}
