using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    public Vector3 muzzleOffset;
    public Projectile projectile;

    public Projectile loadedProjectile
    {
        get { return m_loadedProjectile; }
    }

    protected Projectile m_loadedProjectile = null;
    protected ObjectPooler<Projectile> m_projectilePool;

    private void Start()
    {
        m_projectilePool = new ObjectPooler<Projectile>();
        m_projectilePool.Initialize(10, projectile);
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
        m_loadedProjectile = null; //�ϴ� �߻�Ǹ� �� �̻� �� ��ü�� �߻�ü�� �����ϴ� ���� �ƴ϶� �߻�ü�� ������ ������ �����մϴ�.
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
