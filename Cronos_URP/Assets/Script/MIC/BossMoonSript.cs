using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoonSript : MonoBehaviour
{
    public float fallSpeed = 1.0f;
    bool fall = false;

    public GameObject ring;
    public GameObject ring2;

    public ParticleSystem ringPs;
    public ParticleSystem ring2Ps;
    ParticleSystem.MainModule rMainMod;
    ParticleSystem.MainModule rMainMod2;
    public GameObject blackHoleParticle;
    public GameObject blackHoleSphere;
    Color blackColor = Color.black;
    Color blueColor = Color.blue;
    Color yellowColor = Color.yellow;
    Color redColor = Color.red;
    Color whiteColor = Color.white;
    public float changeDuration = 1.0f;
    public float colorDuration = 3.0f;
    float count = 0;
    Material mat;
    

    void Start()
    {
        rMainMod = ringPs.main;
        rMainMod2 = ring2Ps.main;
        ring.SetActive(false);
        ring2.SetActive(false);
        blackHoleParticle.SetActive(false);
        mat = blackHoleSphere.gameObject.GetComponent<Renderer>().material;

        Invoke("MoonFall", 1.0f);
        //Destroy(gameObject, 28.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (fall && transform.position.y > 0.5f)
        {
            transform.position -= new Vector3(0, fallSpeed * Time.deltaTime, 0);
        }
        else if (fall && transform.position.y <= 0.5f)
        {
            fall = false;
            StartCoroutine(ChangeColorCoroutine(blackColor, blueColor));
        }
    }

    public void CallBlast()
    {
        GameObject boom = EffectManager.Instance.SpawnEffect("BossFX_MoonBlast", transform.position);
        Destroy(boom, 2.0f);
        Destroy(gameObject);
    }

    public void MoonFall()
    {
        fall = true;
    }


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

            Color curColor = Color.Lerp(startColor, endColor, elapsedTime/changeDuration);
            Color rimColor = Color.Lerp(whiteColor, endColor, elapsedTime / changeDuration);

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

		// 색이 다바뀐다음에 넣고 싶으면
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(endColor, 0.0f), new GradientColorKey(endColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );

        col1.color = new ParticleSystem.MinMaxGradient(gradient);
        col2.color = new ParticleSystem.MinMaxGradient(gradient);

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
