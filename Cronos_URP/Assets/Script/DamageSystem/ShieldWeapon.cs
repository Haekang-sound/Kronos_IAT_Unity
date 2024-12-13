using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
