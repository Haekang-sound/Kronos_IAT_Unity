using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class DissolveInstancing : MonoBehaviour
{
    // 몇 초 있다 디졸브 될 것인가
    public float delaySec = 2.0f;

    // 디졸브 될 초
    [SerializeField]
    private float dissolveTime = 0f;

    public Material mat;
    int dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        DoVanish();
    }

    public void DoVanish()
    {
        StartCoroutine(Vanish(delaySec));
    }

    IEnumerator Vanish(float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0.0f;
        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime / dissolveTime));
            mat.SetFloat(dissolveAmount, lerpDissolve);

            yield return null;
        }

        // 부모 째로 없앤다
        //gameObject.SetActive(false);
        DestroyTopParent();   
    }

    // 최상위 부모까지 탐색해서 관련 오브젝트를 디스트로이
    void DestroyTopParent()
    {
        Transform curTrans = transform;

        while (curTrans.parent != null)
        {
            curTrans = curTrans.parent;
        }

        if (curTrans != null)
        {
            Destroy(curTrans.gameObject);
        }
    }
}
