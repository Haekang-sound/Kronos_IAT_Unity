using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParryDamager : SimpleDamager
{
    [SerializeField]
    private bool _isGuard;
    private bool _isParrying;
    private bool _canBeParryied;

    public UnityEvent beParryied;

    private void OnTriggerEnter(Collider other)
    {
        bool isParryied = ParryiedCheck(other);

        if (isParryied == false)
        {
            DamageCheck(other);
        }
    }

    public void BeginGuard()
    {
        _isGuard = true;
    }

    public void EndGuard()
    {
        _isGuard = false;
        EndPerfectGuard();
        EndBeParried();
    }

    public void BeginBeParried()
    {
        _canBeParryied = true;
    }
    public void EndBeParried()
    {
        _canBeParryied = false;
    }

    public void BeginPerfectGuard()
    {
        _isParrying = true;
    }

    public void EndPerfectGuard()
    {
        _isParrying = false;
    }

    public bool ParryiedCheck(Collider other)
    {
        var otherParryDamager = other.GetComponent<ParryDamager>();

        if (otherParryDamager == null ||
            otherParryDamager._isParrying == false ||
            _canBeParryied == false)
        {
            return false;
        }

        beParryied.Invoke();
        return true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        if (_isGuard == true)
        {
            if (_isParrying == true)
            {
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            }
            else
            {
                // Blue
                Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
            }
        }
        else if (_isGuard == false)
        {

            if (_canBeParryied == true)
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
            }
            else
            {
                // Red
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            }
        }

        DrawGizmos();
    }
}
