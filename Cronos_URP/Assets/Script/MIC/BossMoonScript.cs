using System.Collections;
using UnityEngine;

/// <summary>
/// 보스 블랙홀 스킬을 작동시키는 클래스
/// 블랙홀 하나에 붙어서 파티클 색을 바꾸고 그런다.
/// </summary>
public class BossMoonScript : MonoBehaviour
{
	public float fallSpeed = 1.0f;
	bool fall = false;

	public GameObject ring;
	public GameObject ring2;

	public ParticleSystem ringPs;
	public ParticleSystem ring2Ps;
	private ParticleSystem.MainModule rMainMod;
	private ParticleSystem.MainModule rMainMod2;
	public GameObject blackHoleParticle;
	public GameObject blackHoleSphere;
	private Color blackColor = Color.black;
	private Color blueColor = Color.blue;
	private Color yellowColor = Color.yellow;
	private Color redColor = Color.red;
	private Color whiteColor = Color.white;
	public float changeDuration = 1.0f;
	public float colorDuration = 3.0f;
	private float count = 0;
	private Material mat;
	private SoundManager sm;

	// 여기 넣어주면 좋지 않아? ㅎ
	public GameObject gField;
	public GravityField gravityField;

	void Start()
	{
		rMainMod = ringPs.main;
		rMainMod2 = ring2Ps.main;
		ring.SetActive(false);
		ring2.SetActive(false);
		blackHoleParticle.SetActive(false);
		mat = blackHoleSphere.gameObject.GetComponent<Renderer>().material;

		Invoke("MoonFall", 1.0f);
		sm = SoundManager.Instance;
	}

	// Update is called once per frame
	void Update()
	{
		if (fall && transform.position.y > -2f)
		{
			transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
		}
		else if (fall && transform.position.y <= -2f)
		{
			fall = false;
			sm.PlaySFX("Boss_BlackHole_Drop_Sound_SE", transform);
			gField.SetActive(true);
			StartCoroutine(ChangeColorCoroutine(blackColor, blueColor));
            sm.PlaySFX("Boss_BlackHole_Base_Sound_SE", transform);
        }
	}

	// 뻥
	public void CallBlast()
	{
		ImpulseCam.Instance.Shake(ImpulseCam.Instance.blackHoleStrength);
		GameObject boom = EffectManager.Instance.SpawnEffect("BossFX_MoonBlast", transform.position);
		
        sm.PlaySFX("Boss_BlackHole_Bomb_Sound_SE", transform);
		GameObject boomDamage = EffectManager.Instance.SpawnEffect("BossFX_GravityField_Damager", transform.position);
		Destroy(boomDamage,0.5f);
		Destroy(boom, 2.0f);
		Destroy(gameObject);
	}

	public void MoonFall()
	{
		fall = true;
	}

	// start컬러에서 end컬러로 파티클이랑 림라이트까지
	public IEnumerator ChangeColorCoroutine(Color startColor, Color endColor)
	{
		// 서서히 바뀔때 이때 호출 하면됨
		float elapsedTime = 0.0f;

		ring.SetActive(true);
		ring2.SetActive(true);

		var col1 = ringPs.colorOverLifetime;
		var col2 = ring2Ps.colorOverLifetime;
		col1.enabled = true;
		col2.enabled = true;

		Gradient gradient = new Gradient();

		while (elapsedTime < changeDuration)
		{
			elapsedTime += Time.deltaTime;

			Color curColor = Color.Lerp(startColor, endColor, elapsedTime / changeDuration);
			Color rimColor = Color.Lerp(whiteColor, endColor, elapsedTime / changeDuration);

			/// 점점 바뀌게 해볼까? 
			if (endColor == blueColor)
			{
				gravityField.AnimSpeed = Mathf.Lerp(1f, 2f, elapsedTime / changeDuration);
			}
			else if (endColor == yellowColor)
			{
				gravityField.AnimSpeed = Mathf.Lerp(1f, 0.1f, elapsedTime / changeDuration);
			}

			// gradient 업데이트
			gradient.SetKeys(
				new GradientColorKey[] { new GradientColorKey(curColor, 0.0f), new GradientColorKey(curColor, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
				);

			col1.color = new ParticleSystem.MinMaxGradient(gradient);
			col2.color = new ParticleSystem.MinMaxGradient(gradient);
			if (startColor == Color.black)
				mat.SetColor("_RimLightColor", rimColor);
			else if (endColor == Color.black)
				mat.SetColor("_RimLightColor", redColor);
			else
				mat.SetColor("_RimLightColor", curColor);
			//Debug.Log("Current rim Light : " + mat.GetColor("_RimLightColor"));

			yield return null;
		}

		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(endColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
		);
		col1.color = new ParticleSystem.MinMaxGradient(gradient);
		col2.color = new ParticleSystem.MinMaxGradient(gradient);

		// 색이 다바뀐다음에 넣고 싶으면
		// 		if (endColor == blueColor)
		// 		{
		// 			gravityField.AnimSpeed = 0.1f;
		// 		}
		// 		else if (endColor == yellowColor)
		// 		{
		// 			gravityField.AnimSpeed = 10f;
		// 		}
		// 		else if (endColor == redColor)

		if (endColor == redColor)
		{
			//안쪽으로 몰려~
			gravityField.AnimSpeed = 1f;
			gravityField.isGravitation = true;

		}

		count++;

		// 유지시간
		yield return new WaitForSeconds(colorDuration);

		if (count < 5 && endColor == blueColor)
		{
			StartCoroutine(ChangeColorCoroutine(blueColor, yellowColor));
		}
		else if (count < 5 && endColor == yellowColor)
		{
			StartCoroutine(ChangeColorCoroutine(yellowColor, blueColor));
		}
		else if (count == 5)
		{
			StartCoroutine(ChangeColorCoroutine(endColor, redColor));
			blackHoleParticle.SetActive(true);
		}
		else
		{
			//Debug.Log("enough counts");
			yield return new WaitForSeconds(5.0f);
			StartCoroutine(ChangeColorCoroutine(redColor, blackColor));
            yield return new WaitForSeconds(1.0f);
			CallBlast();
		}
	}
}
