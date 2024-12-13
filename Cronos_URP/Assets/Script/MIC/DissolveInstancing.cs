using System.Collections;
using UnityEngine;


/// <summary>
/// 디졸브 마테리얼 스크립트
/// 마테리얼을 개체마다 인스턴스화하고 사라지는 것처럼 만든다
/// 디졸브가 끝나면 랙돌째로 Destroy한다
/// </summary>
public class DissolveInstancing : MonoBehaviour
{
    [SerializeField]
    private float delaySec = 2.0f;

    [SerializeField]
    private float dissolveTime = 1.0f;

    private Material mat;
    int dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void Start()
    {
        // 마테리얼 인스턴싱
        mat = gameObject.GetComponent<Renderer>().material;
        DoVanish();
    }

    public void DoVanish()
    {
        StartCoroutine(Vanish(delaySec));
    }

    // 이걸로 디졸브
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
