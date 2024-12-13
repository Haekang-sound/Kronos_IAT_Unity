using UnityEngine;

/// <summary>
/// ���Ÿ� ������ �⺻ Ŭ������, �߻�ü�� �߻��ϰ�, ��ü Ǯ���� �߻�ü�� �����ϴ� ����� �����մϴ�.
/// �߻�ü�� `Projectile` Ŭ������ ����Ͽ� �����Ǹ�, �߻� �� ȿ�� �� �Ҹ��� ó���մϴ�.
/// </summary>
public class RangeWeapon : MonoBehaviour
{
    public Vector3 muzzleOffset;
    public Projectile projectile;
    public int pooledObejctNum;

    SoundManager sm;

    public Projectile loadedProjectile
    {
        get { return _loadedProjectile; }
    }

    protected Projectile _loadedProjectile = null;
    protected ObjectPooler<Projectile> _projectilePool;

    private void Start()
    {
        _projectilePool = new ObjectPooler<Projectile>();
        _projectilePool.Initialize(pooledObejctNum, projectile);
        sm = SoundManager.Instance;
    }

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
        if (_loadedProjectile != null)
        {
            return;
        }

        _loadedProjectile = _projectilePool.GetNew();
        _loadedProjectile.transform.SetParent(transform, false);
        _loadedProjectile.transform.localPosition = muzzleOffset;
        _loadedProjectile.transform.localRotation = Quaternion.identity;
    }

    void AttackProjectile(Vector3 target)
    {
        if (_loadedProjectile == null)
        {
            LoadProjectile();
        }

        _loadedProjectile.transform.SetParent(null, true);
        _loadedProjectile.Shot(target, this);
        _loadedProjectile = null; //�ϴ� �߻�Ǹ� �� �̻� �� ��ü�� �߻�ü�� �����ϴ� ���� �ƴ϶� �߻�ü�� ������ ������ �����մϴ�.
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
