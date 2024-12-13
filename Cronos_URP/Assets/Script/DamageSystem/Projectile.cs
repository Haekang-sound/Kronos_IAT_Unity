using UnityEngine;

/// <summary>
/// 발사체의 추상 클래스입니다. 이 클래스는 발사체의 기본 기능을 정의하며, 
/// 객체 풀링 시스템을 위해 `IPooled` 인터페이스를 구현합니다.
/// </summary>
public abstract class Projectile : MonoBehaviour, IPooled<Projectile>
{
    public int poolID { get; set; }
    public ObjectPooler<Projectile> pool { get; set; }

    public abstract void Shot(Vector3 target, RangeWeapon shooter);
}