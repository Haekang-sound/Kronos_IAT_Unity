using UnityEngine;

/// <summary>
/// 방패 무기의 기능을 담당하는 클래스입니다.
/// 이 클래스는 방패의 가드와 패리 기능을 처리하며, `ParryDamager`를 통해 방패의 상태를 제어합니다.
/// </summary>
public class ShieldWeapon : MonoBehaviour
{
    private ParryDamager _damager;

    private void Awake()
    {
        _damager = GetComponentInChildren<ParryDamager>();
    }

    public void BeginGuard()
    {
        _damager.gameObject.SetActive(true);
        _damager.BeginGuard();
    }

    public void EndGuard()
    {
        _damager.EndGuard();
        _damager.gameObject.SetActive(false);
    }

    public void BeginParry()
    {
        _damager.BeginPerfectGuard();
    }

    public void EndParry()
    {
        _damager.EndPerfectGuard();
    }
}
