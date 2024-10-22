using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DissolveInstancings : MonoBehaviour
{
    // �� �� �ִ� ������ �� ���ΰ�
    public float delaySec = 2.0f;

    public UnityEvent Vanished;

    // ������ �� ��
    [SerializeField]
    private float dissolveTime = 6f;

    int dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    public void DoVanish()
    {
        StartCoroutine(Vanish(delaySec));
    }

    IEnumerator Vanish(float delay)
    {
        SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        yield return new WaitForSeconds(delay);

        float elapsedTime = 0.0f;
        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime / dissolveTime));
            

            foreach (SkinnedMeshRenderer render in renderers)
            {
                render.material.SetFloat(dissolveAmount, lerpDissolve);
            }

            yield return null;
        }

        Vanished?.Invoke();

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
