using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


/// <summary>
/// 이펙트를 관리하는 매니저
/// 하는 역할은 이펙트를 인스턴스화해서 뽑고 지우는거
/// 모든 이펙트 프리팹의 스폰은 여기에서 한다
/// </summary>
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
	public GameObject pSword;

	private SoundManager soundManager;
	private ImpulseCam ic;

	// 레이캐스트 관련
	float forwardVal = 1.6f;
	float yUpVal = 1.5f;
	public LayerMask groundLayer;
	public float rayMaxDist = 2.0f;

	// 글로벌 볼륨
	[SerializeField]
	private Volume eVolume;
	private MotionBlur mBlur;
	private ChromaticAberration cAber;
	private DepthOfField dOF;
	public float mBlurVal = 1.0f;
	public float cAberVal = 0.7f;
	public float dofFDistance = 2.0f;
	public float dofFLength = 100.0f;
	public float dofAperture = 6.0f;

	public float parryTime = 1.0f;
	public float fadeTime = 0.3f;

	// 캐릭터 평타 각도를 맞추기 위해서
	private Vector3 swordMagicOffest = new Vector3(90, 180, 0);
	private Vector3 enemyMagicOffset = new Vector3(0, 90, 0);
	private Vector3 bossMagicOffset = new Vector3(0, 90, 70);

	// 강화 검기 관련
	public bool isSwordWave;
	[Range(0f, 200f)]
	public float enforceSlashSpeed = 30.0f;
	public float swordWaveSpeed = 20.0f;
	public float swordWaveDistance = 12.0f;
	public GameObject invisibleSlash;
	public bool isGroundEnforced;
	public bool showInvisibleMesh;

	// 강화 상태 오우라
	public GameObject swordAura;

	// 파티클을 빨아들이는 박스
	public GameObject absorbBox;

	// 보스 이펙트 관련
	public float bossBeamDistance = 4.0f;
	public float bossBeamTerm = 0.2f;
	public float bossBeamDupeTime = 8.0f;

	public float bossMoonHeight = 4.0f;
	public float bossMoonDistance = 5.0f;
	public Vector3 bossMoonScale;
	public Transform bossMoontarget;
	private Vector3 bossFireOffset = new Vector3(0, 0, 3.0f);

	// 사용할 이펙트 리스트
	private GameObject[] effectArray;

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

	/// <summary>
	/// 씬전환 초기화를 위한 함수
	/// 믹동휘는 고치던가 
	/// By ohk    v1
	/// </summary>
	//private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	//{
	//	//Initialize();
	//}

	
	void Start()
	{
		Initialize();
		StartCoroutine(LoadEffectCoroutine());
	}

	void Update()
	{
		// 오씨가 만든 올킬 스킬
		if (Input.GetKey(KeyCode.K)
			&&(Input.GetKey(KeyCode.I))
			&&(Input.GetKeyDown(KeyCode.L)))
		    QASkill();
		
	}

	private void OnValidate()
	{
		if (invisibleSlash != null)
		{
			ToggleMeshRenderer();
		}
	}

	// 필요한 것들 할당하기
	void Initialize()
	{
		soundManager = SoundManager.Instance;
		player = Player.Instance;
		ic = ImpulseCam.Instance;

		if (player != null)
		{
			pSword = player.GetComponent<Player>().playerSword;
		}

		if (eVolume == null)
		{
			// 비용이 크다지만 볼륨이 없으면 안되니까
			GameObject eVol = GameObject.Find("Effect Volume");
			eVolume = eVol.GetComponent<Volume>();
		}

		if (eVolume != null)
		{
			InitializeVol(eVolume);
		}

		groundLayer = LayerMask.GetMask("Ground");

		if (invisibleSlash != null)
		{
			ToggleMeshRenderer();
		}

		if (swordAura == null)
		{
			swordAura = GameObject.Find("Com_Ready");
		}

		if (swordAura != null)
		{
			swordAura.SetActive(false);
		}

		if (absorbBox == null)
		{
			absorbBox = GameObject.Find("AbsorbBox");
		}
	}

	void InitializeVol(Volume vol)
	{
		vol.profile.TryGet(out mBlur);
		vol.profile.TryGet(out cAber);
		vol.profile.TryGet(out dOF);

		if (mBlur != null)
		{
			mBlur.active = true;
			mBlur.intensity.value = 0.0f;
		}

		if (cAber != null)
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

    // 로드한 이펙트에 게임 오브젝트 할당
    private IEnumerator LoadEffectCoroutine()
	{
		effectArray = Resources.LoadAll<GameObject>("Prefabs/FX");
		foreach (GameObject effect in effectArray)
		{
			GameObject effectInstance = Instantiate(effect);
			effectInstance.name = effect.name;
			//effects.Add(effectInstance);
			effectInstance.SetActive(false);

			yield return null;
		}
	}

	// 거의 모든 이펙트는 이걸 통해서
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

	// 오버로딩 1 : 플레이어로 가는 파티클의 개수를 변경하고 싶을 때
	public GameObject SpawnEffect(string name, Vector3 pos, float value)
	{
		foreach (GameObject effect in effectArray)
		{
			if (effect.name == name)
			{
				GameObject instance = Instantiate(effect);
				instance.transform.position = pos;
				if (instance.GetComponent<AheadToPlayer>() != null)
				{
					instance.GetComponent<AheadToPlayer>().BurstNumber = value;
				}

				return instance;
			}
		}

		return null;
	}

	// 오버로딩 2 : 프리팹이 돌아가있어서 회전해서 뽑아야 할 때
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


	// 부모 오브젝트에서 이름을 가진 자식 오브젝트를 리턴
	// 이펙트가 나올 자식 오브젝트 위치 찾는 데 사용하는 중
	GameObject FindChild(GameObject parent, string name)
	{
		GameObject result = GameObject.Find(name);
		if (result != null)
		{
			return result;
		}

		foreach (GameObject gameObject in parent.GetComponentsInChildren<GameObject>())
		{
			result = FindChild(gameObject, name);
			if (result != null)
				return result;
		}

		return null;
	}

	// 투명 투사체 오브젝트를 다시 그리냐 마냐
	void ToggleMeshRenderer()
	{
		if (invisibleSlash != null)
		{
			// 프리팹 인스턴스에서 MeshRenderer를 찾고 활성화/비활성화
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

	// 플레이어 관련 이펙트
	public void NormalSlashFX(string fxName)
	{
		// 이펙트 뽑고 로테이션을 칼의 로테이션과 맞춘다.
		// 칼과 이펙트의 기준이 다르므로 이건 이펙트마다 매직 넘버가 필요함
		// 위치는 y 좌표만 칼과 같게, 나머지는 플레이어 트랜스폼에서
		GameObject slash = SpawnEffect(fxName, player.transform.position);
		slash.transform.rotation = player.playerSword.transform.rotation * Quaternion.Euler(swordMagicOffest);
		float newY = player.playerSword.transform.position.y;
		slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
		Destroy(slash, 0.7f);
	}

	// 일반 강공격 스핀
	public void NormalStrongFX()
	{
		GameObject slash = SpawnEffect("Nor_S_Attack", player.transform.position);
		float newY = player.playerSword.transform.position.y;
		slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
		Destroy(slash, 0.7f);
	}

	// 강화 오라 활성화
	public void SwordAuraOn()
	{
		swordAura.SetActive(true);
	}

	public void SwordAuraOff()
	{
		swordAura.SetActive(false);
	}

	// 구르기 공격
	public void DodgeAttack()
	{
		soundManager.PlaySFX("Attack_SE", player.transform);
		GameObject dttack = SpawnEffect("DodgeAttack", player.transform.position);
		float newY = player.playerSword.transform.position.y;
		dttack.transform.position = new Vector3(dttack.transform.position.x, newY, dttack.transform.position.z);
		dttack.transform.forward = player.transform.forward;
		Destroy(dttack, 4.0f);
	}

	// 어빌리티 공격 1
	public void AbilitySlash()
	{
		//soundManager.PlaySFX("Attack_SE", player.transform);
		GameObject aSlash = SpawnEffect("AbilitySlash", player.transform.position);
		aSlash.transform.rotation = player.playerSword.transform.rotation;
		float newY = player.playerSword.transform.position.y;
		aSlash.transform.position = new Vector3(aSlash.transform.position.x, newY, aSlash.transform.position.z);
		Destroy(aSlash, 2.0f);
	}

	// 적의 근접 기본공격
	public void EnemySlash(Transform enemy)
	{
		// 이펙트 뽑고 로테이션을 칼의 로테이션과 맞춘다.
		// 칼과 이펙트의 기준이 다르므로 이건 이펙트마다 매직 넘버가 필요함
		// 위치는 y 좌표만 칼과 같게, 나머지는 플레이어 트랜스폼에서
		GameObject slash = SpawnEffect("EnemyAttack", enemy.position);
		slash.transform.rotation = enemy.gameObject.GetComponent<ATypeEnemyBehavior>().
			enemySword.transform.rotation * Quaternion.Euler(enemyMagicOffset);
		float newY = enemy.gameObject.GetComponent<ATypeEnemyBehavior>().
			enemySword.transform.position.y;
		slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
		Destroy(slash, 0.7f);
	}

	// 보스 슬래시가 없더라
	public void BossSlash(Transform trans)
	{
		GameObject slash = SpawnEffect("BossSlash", trans.position);
		slash.transform.rotation = trans.gameObject.GetComponent<BossBehavior>().
			bossSword.transform.rotation * Quaternion.Euler(bossMagicOffset);
		float newY = trans.gameObject.GetComponent<BossBehavior>().
			bossSword.transform.position.y;
		slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
		Destroy(slash, 0.7f);
	}

	// 적 근접 돌진공격
	public void EnemyCharge(Transform enemy)
	{
		GameObject charge = SpawnEffect("EnemyCharge", enemy.position);
		charge.transform.position += new Vector3(0, 1f, 0);
		charge.transform.forward = enemy.forward;
		Destroy(charge, 2.0f);
	}

	public void QASkill()
	{
		// 레이를 쏠 위치 플레이어의 위치 + 정면으로 조금 앞으로 + 조금 위로
		// 기준을 플레이어 포워드에서 칼 로컬 중심으로 조금 바꿈
		Vector3 rayTrans = player.transform.position +
			pSword.transform.up * -1 * forwardVal +
			new Vector3(0, yUpVal, 0);
		Debug.DrawRay(rayTrans, Vector3.down * rayMaxDist, Color.yellow, 1.0f);
		if (Physics.Raycast(rayTrans, Vector3.down, out RaycastHit hit, rayMaxDist, groundLayer))
		{
			Vector3 hitPoint = hit.point;
			Vector3 hitNormal = hit.normal;
			// ProjectOnPlane은 첫번째 매개변수 벡터를 두번째 매개변수 노말에 투영된 벡터를 반환한다. 
			Quaternion fxRot = Quaternion.LookRotation(
				//Vector3.ProjectOnPlane(player.transform.forward, hitNormal), hitNormal);
				Vector3.ProjectOnPlane(pSword.transform.up * -1, hitNormal), hitNormal);
			fxRot *= Quaternion.Euler(0, -90f, 0);
			GameObject impact = SpawnEffect("Nor04_Ground", hitPoint, fxRot);
			soundManager.PlaySFX("Ground_Impact_2_Sound_SE", player.transform);

			Destroy(impact, 2.0f);
			// 능력개방되었다면 이것도 나옴
			GameObject cir = SpawnEffect("AllKillFORQA", hitPoint, fxRot);
			Destroy(cir, 1.0f);
		}

		else
		{
			Debug.Log("no ground impact");
		}
	}

	// 지면의 각도에 맞게 이펙트를 남기려면 어떻게 해야할까
	public void GroundCheckFX()
	{
		// 레이를 쏠 위치 플레이어의 위치 + 정면으로 조금 앞으로 + 조금 위로
		// 기준을 플레이어 포워드에서 칼 로컬 중심으로 조금 바꿈
		Vector3 rayTrans = player.transform.position +
			//player.transform.forward * forwardVal + 
			pSword.transform.up * -1 * forwardVal +
			new Vector3(0, yUpVal, 0);
		Debug.DrawRay(rayTrans, Vector3.down * rayMaxDist, Color.yellow, 1.0f);
		if (Physics.Raycast(rayTrans, Vector3.down, out RaycastHit hit, rayMaxDist, groundLayer))
		{
			Vector3 hitPoint = hit.point;
			Vector3 hitNormal = hit.normal;
			// ProjectOnPlane은 첫번째 매개변수 벡터를 두번째 매개변수 노말에 투영된 벡터를 반환한다. 
			Quaternion fxRot = Quaternion.LookRotation(
				//Vector3.ProjectOnPlane(player.transform.forward, hitNormal), hitNormal);
				Vector3.ProjectOnPlane(pSword.transform.up * -1, hitNormal), hitNormal);
			fxRot *= Quaternion.Euler(0, -90f, 0);
			GameObject impact = SpawnEffect("Nor04_Ground", hitPoint, fxRot);
			soundManager.PlaySFX("Ground_Impact_2_Sound_SE", player.transform);

			Destroy(impact, 2.0f);
			// 능력개방되었다면 이것도 나옴
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

	// 바닥에 상처만 남기는 Nor_Attack_4 전용
	// 이었는데 이제 능력개방에서도 써서 변수 받아야됨
	public void GroundScar(string name)
	{
		Debug.Log("땅 체크 시작");
		Vector3 rayTrans = player.transform.position +
			pSword.transform.up * -1 * forwardVal +
			new Vector3(0, yUpVal, 0);
		Debug.DrawRay(rayTrans, Vector3.down * rayMaxDist, Color.yellow, 1.0f);
		if (Physics.Raycast(rayTrans, Vector3.down, out RaycastHit hit, rayMaxDist, groundLayer))
		{
			Debug.Log("범위에 땅이 있다");
			Vector3 hitPoint = hit.point;
			Vector3 hitNormal = hit.normal;
			// ProjectOnPlane은 첫번째 매개변수 벡터를 두번째 매개변수 노말에 투영된 벡터를 반환한다. 
			Quaternion fxRot = Quaternion.LookRotation(
				Vector3.ProjectOnPlane(pSword.transform.up * -1, hitNormal), hitNormal);
			fxRot *= Quaternion.Euler(0, -90f, 0);
			GameObject impact = SpawnEffect(name, hitPoint, fxRot);
			soundManager.PlaySFX("Ground_Impact_2_Sound_SE", player.transform);
			Destroy(impact, 2.0f);
		}

		else
		{
			Debug.Log("no ground impact");
		}
	}

	public void BossScar(Transform trans)
	{
		GameObject impact = SpawnEffect("BossGrounder", trans.position - new Vector3(0, 0.7f, 0));
		soundManager.PlaySFX("Ground_Impact_2_Sound_SE", transform);
		Destroy(impact, 1.5f);
	}

	// 검기 날리기
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
		StartCoroutine(DestroyWaveCoroutine(proj));

		while (proj != null && Vector3.Distance(proj.transform.position, startPos) < swordWaveDistance)
		{
			if (proj == null)
			{
				yield break;
			}

			proj.transform.position = Vector3.MoveTowards(proj.transform.position, DestPos, swordWaveSpeed * Time.deltaTime);

			yield return null;
		}
	}

	IEnumerator DestroyWaveCoroutine(GameObject wav)
	{
		yield return new WaitForSeconds(0.7f);
		if (wav != null)
		{
			Destroy(wav);
		}
	}

	// 이펙트매니저가 들고 있는게 나을 것 같은데
	public void CreateHitFX(Damageable.DamageMessage dmgMsg, Transform targetTrans)
	{
		// 파편만들기
		GameObject frag = SpawnEffect("FragFX", targetTrans.position);
		frag.transform.LookAt(dmgMsg.damageSource);
		frag.transform.Rotate(-15f, 0, 0);
		Destroy(frag, 2.0f);

		CreateAbsorbFX(targetTrans, dmgMsg.amount);

		// 피격이펙트
		Vector3 newPos = new Vector3(targetTrans.position.x - dmgMsg.direction.x, dmgMsg.damageSource.y, targetTrans.position.z);
		GameObject slashed = SpawnEffect("Nor_Damage", newPos);
		slashed.transform.forward = Camera.main.transform.forward;
		slashed.transform.position += new Vector3(0, 1, 0);
		slashed.transform.rotation = player.playerSword.transform.rotation *
			Quaternion.Euler(new Vector3(90f, 0, 0));
		Destroy(slashed, 1.0f);
	}

	public void CreateHitFX(Damageable.DamageMessage dmgMsg, Transform targetTrans, bool isSpawner)
	{
		// 파편만들기
		GameObject frag = SpawnEffect("FragFX", targetTrans.position);
		frag.transform.LookAt(dmgMsg.damageSource);
		frag.transform.Rotate(-15f, 0, 0);
		Destroy(frag, 2.0f);

		if (!isSpawner)
		{
			CreateAbsorbFX(targetTrans, dmgMsg.amount);
		}

		// 피격이펙트
		Vector3 newPos = new Vector3(targetTrans.position.x - dmgMsg.direction.x, dmgMsg.damageSource.y, targetTrans.position.z);
		GameObject slashed = SpawnEffect("Nor_Damage", newPos);
		slashed.transform.forward = Camera.main.transform.forward;
		slashed.transform.position += new Vector3(0, 1, 0);
		slashed.transform.rotation = player.playerSword.transform.rotation *
			Quaternion.Euler(new Vector3(90f, 0, 0));
		Destroy(slashed, 1.0f);
	}

	// 플레이어가 맞을 때 나오는 이펙트
	public void PlayerHitFX(Damageable.DamageMessage dmgMsg)
	{
		Vector3 newPos = new Vector3(player.transform.position.x - dmgMsg.direction.x, dmgMsg.damageSource.y, player.transform.position.z);
		GameObject pHit = SpawnEffect("Eff_Player_Damage", newPos);
		soundManager.PlaySFX("Hit_SE", player.transform);
		pHit.transform.position += new Vector3(0, 1, 0);
		pHit.transform.forward = dmgMsg.direction;
		Destroy(pHit, 1.0f);
	}

	public void CreateAbsorbFX(Transform trans, float burstNum)
	{
		// 1초 뒤에 플레이어로 돌진하는 파티클 만들기
		GameObject ab = SpawnEffect("AbsorbFX", trans.position + new Vector3(0, 2, 0), burstNum);
		Destroy(ab, 3.0f);
	}

	public void CreateGuardFX()
	{
		soundManager.PlaySFX("Player_Block_Sound_SE", player.transform);
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

		// 글로벌볼륨이 없다면 나가
		if (eVolume == null)
		{
			return;
		}

		StartCoroutine(ParryMotionBlurCoroutine(mBlurVal));
		StartCoroutine(ParryCAberrationCoroutine(cAberVal));
		StartCoroutine(ParryDepthOfFieldCoroutine());
		StartCoroutine(ParryTime(0.2f));

	}

	public void BossDown(Transform t)
	{
		GameObject down = SpawnEffect("DownSlash", t.position);
		Destroy(down, 0.5f);
	}

	// 보스 8방향 빔
	public IEnumerator BossEightBeamCoroutine(Transform bossTrans)
	{
		ic.Shake(ic.rayStrength);

		for (int i = 0; i < bossBeamDupeTime; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				GameObject beam = SpawnEffect("BossFX_1Beam", bossTrans.position);
				beam.transform.Rotate(0, 45.0f * j, 0);

				GameObject beamBase = beam.transform.GetChild(0).gameObject;
				beamBase.transform.position += beamBase.transform.right * (bossBeamDistance * i);
				//soundManager.PlaySFX("Boss_Ground_Impact_Sound_SE", player.transform);
				Destroy(beam, 0.6f);
			}

			yield return new WaitForSeconds(bossBeamTerm);
		}
	}

	// 보스 불 장판 만들기
	public void BossFireShoot(Transform bosstrans)
	{
		GameObject fire = SpawnEffect("BossFX_FireProjectile", bosstrans.position);
		soundManager.PlaySFX("Boss_Flame_Sound_SE", bosstrans);
		fire.transform.forward = bosstrans.transform.forward;
		fire.transform.position += bosstrans.forward * 8.0f + new Vector3(0, 1.0f, 0);
	}

	// 보스 창 5개 쏘기
	public void BossFiveSpear(Transform bossTrans)
	{
		Vector3 forward = bossTrans.forward;
		Vector3 newOffset = new Vector3(0, 3.5f, 0);
		Vector3 newPos = bossTrans.TransformPoint(newOffset);
		GameObject spears = SpawnEffect("BossFX_Spears", newPos);
		spears.transform.forward = forward;
		Destroy(spears, 15.0f);
	}

	// 창 지면 이펙트
	public void SpearImpact(Vector3 pos)
	{
		GameObject imp = SpawnEffect("BossFX_SpearImpact", pos);
		Destroy(imp, 3.0f);
	}

	// 보스 위성 만들기
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

	public void BossMoonFixedPosition()
	{
		if (bossMoontarget == null)
		{
			return;
		}

		List<int> moonNums = new List<int>();
		moonNums = FisherYatesShuffles(8, 5);
		Vector3 newOffset = new Vector3(0, bossMoonHeight, 0);
		Vector3 newPos = bossMoontarget.TransformPoint(newOffset);

		for (int i = 0; i < moonNums.Count; i++)
		{
			GameObject moon = SpawnEffect("BossFX_BlackHole", newPos);
			moon.transform.Rotate(0, 45.0f * moonNums[i], 0);
			moon.transform.position += moon.transform.forward * bossMoonDistance;
			moon.transform.localScale += bossMoonScale;
		}

	}


	// 집중선 켰다가 끄기
	IEnumerator SpeedLineCoroutine()
	{
		UI_TPCPHUD.Instance.speedLineUI.SetActive(true);

		yield return new WaitForSeconds(1.5f);

		UI_TPCPHUD.Instance.speedLineUI.SetActive(false);
	}

	public void SpeedLine()
	{
		StartCoroutine(SpeedLineCoroutine());
	}


	// 패리했을 때 모션 블러
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

	// 패리했을 때 크로마틱 애버레이션
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

	// 패리했을 때 뎁스 오브 필드 시간은 다른 효과보다 조금 빨리 끝나게
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

	// 패리했을 때 시간 줄이기
	IEnumerator ParryTime(float val)
	{
		// val 만큼 타임스케일 줄였다 원래대로 돌리기
		Time.timeScale = val;
		float elapsedTime = 0.0f;
		// n초에 걸쳐서 돌아가기
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


	// elements 개의 원소를 넣으면 result 개의 랜덤 값을 앞에서 뽑는
	// 피셔-예이츠 알고리즘
	public List<int> FisherYatesShuffles(int elements, int result)
	{
		// 0부터 elements개의 숫자를 리스트로 생성
		List<int> numbers = new List<int>();
		for (int i = 0; i < elements; i++)
		{
			numbers.Add(i);
		}

		// 랜덤 객체 생성
		System.Random rand = new System.Random();

		// 피셔-예이츠 알고리즘으로 리스트 셔플
		for (int i = numbers.Count - 1; i > 0; i--)
		{
			int j = rand.Next(0, i + 1); // 0부터 i까지의 인덱스 중 하나를 선택
										 // numbers[i]와 numbers[j]를 swap
			int temp = numbers[i];
			numbers[i] = numbers[j];
			numbers[j] = temp;
		}

		// 결과 출력
		Debug.Log("랜덤 배열: " + string.Join(", ", numbers));

		List<int> numbers2 = new List<int>();
		for (int i = 0; i < result; i++)
		{
			numbers2.Add(numbers[i]);
		}

		Debug.Log("number2 배열: " + string.Join(", ", numbers2));

		return numbers2;
	}
}
