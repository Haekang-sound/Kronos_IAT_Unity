using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    public Vector3 muzzleOffset;
    public Projectile projectile;
    public int pooledObejctNum;

    SoundManager sm;

    public Projectile loadedProjectile
    {
        get { return m_loadedProjectile; }
    }

    protected Projectile m_loadedProjectile = null;
    protected ObjectPooler<Projectile> m_projectilePool;

    private void Start()
    {
        m_projectilePool = new ObjectPooler<Projectile>();
        m_projectilePool.Initialize(pooledObejctNum, projectile);
        sm = SoundManager.Instance;
    }

    // TEST
    //public GameObject target;
    //const float maxtime = 1f;
    //float passedTime = 0;
    //private void Update()
    //{
    //    passedTime -= Time.deltaTime;
    
    //    if (passedTime < 0)
    //    {
    //        passedTime = maxtime;
    //        Attack(target.transform.position);
    //    }
    
    //}

    public void Attack(Vector3 target)
    {
        AttackProjectile(target);
        GameObject shoot = EffectManager.Instance.SpawnEffect("EnemyShooting", transform.position);
        shoot.transform.forward = gameObject.transform.forward;
        shoot.transform.position += new Vector3(0, 1.5f, 0);
        Destroy(shoot, 0.7f);
        sm.PlaySFX("Enemy_Bow_Shoot_Sound_SE", gameObject.transform);
    }

    public void LoadProjectile()
    {
        if (m_loadedProjectile != null)
        {
            return;
        }

        m_loadedProjectile = m_projectilePool.GetNew();
        m_loadedProjectile.transform.SetParent(transform, false);
        m_loadedProjectile.transform.localPosition = muzzleOffset;
        m_loadedProjectile.transform.localRotation = Quaternion.identity;
    }

    void AttackProjectile(Vector3 target)
    {
        if (m_loadedProjectile == null)
        {
            LoadProjectile();
        }

        m_loadedProjectile.transform.SetParent(null, true);
        m_loadedProjectile.Shot(target, this);
        m_loadedProjectile = null; //일단 발사되면 더 이상 이 객체가 발사체를 소유하는 것이 아니라 발사체가 스스로 생명을 유지합니다.
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 worldOffset = transform.TransformPoint(muzzleOffset);
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawLine(worldOffset + Vector3.up * 0.4f, worldOffset + Vector3.down * 0.4f);
        UnityEditor.Handles.DrawLine(worldOffset + Vector3.forward * 0.4f, worldOffset + Vector3.back * 0.4f);
    }
#endif
}
