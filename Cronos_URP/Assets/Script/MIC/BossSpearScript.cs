using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpearScript : MonoBehaviour
{
    public bool act = false;
    public bool sat = false;
    public Player target;
    public Vector3 targetPos;
    public float lookSpeed;
    public float incSpeed = 0.0f;
    public float elapsedTime;
    public float spearDamage = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        target = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어를 향해 조준
        if (act)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, lookSpeed);
            targetPos = target.transform.position;
        }
    }

    // 사출
    public IEnumerator Saturate(float delay)
    {
        yield return new WaitForSeconds(delay);

        act = false;
        sat = true;

        Vector3 dir = (targetPos - transform.position).normalized;
        while (Vector3.Distance(transform.localPosition, targetPos) > 0.01f)
        {
            if (!sat)
            {
                yield break;
            }

            elapsedTime += Time.deltaTime;
            incSpeed += (elapsedTime * elapsedTime);
            transform.position += dir * incSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        int collisionLayer = collision.gameObject.layer;
        if (collisionLayer == LayerMask.NameToLayer("Player"))
        {
            // 플레이어한테 대미지박기
            Debug.Log("충돌 : " + collision.gameObject.name);
            //Player.Instance._damageable.currentHitPoints -= spearDamage;
        }

        if (collisionLayer == LayerMask.NameToLayer("Ground"))
        {
            sat = false;
            Destroy(GetComponent<SimpleDamager>());
            Debug.Log("창 부딪침");
            EffectManager.Instance.SpearImpact(targetPos);
            Destroy(gameObject, 3.0f);
        }
    }
}
