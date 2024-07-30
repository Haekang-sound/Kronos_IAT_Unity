using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class DissolveInstancing : MonoBehaviour
{
    // �� �� �ִ� ������ �� ���ΰ�
    public float delaySec = 2.0f;

    // ������ �� ��
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

        // �θ� °�� ���ش�
        //gameObject.SetActive(false);
        DestroyTopParent();   
    }

    // �ֻ��� �θ���� Ž���ؼ� ���� ������Ʈ�� ��Ʈ����
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
