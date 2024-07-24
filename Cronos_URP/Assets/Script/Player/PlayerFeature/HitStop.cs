using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public float hitStopTime = 0.0f;
    public float hitStopScale = 0.0f;
    Animator animator;
    
	public bool isHit;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
            Debug.Log("�ִϸ����͸� ã�ҽ��ϴ�");            
        isHit = false;
    }

    private void Update()
    {
        
    }

	// ��¥ �̾��ѵ� �ۺ����� �ٲ�
    public IEnumerator HitStopCoroutine()
    {
        if (!isHit)
        {
            isHit = true;
            animator.speed = hitStopScale;
            yield return new WaitForSecondsRealtime(hitStopTime);
            animator.speed = 1.0f;
            isHit = false;
        }
    }
}
