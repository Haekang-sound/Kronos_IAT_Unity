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

    public UnityEvent beParryied, parrying;

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
            otherParryDamager?._isParrying == false ||
            _canBeParryied == false)
        // _canBeParryied == true && otherParryDamager._isParrying == true
        {
            return false;
        }

        beParryied.Invoke();
        otherParryDamager.parrying.Invoke();

        return true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        if (_isGuard == true)
        {
            // Green - �и� ������ ����
            if (_isParrying == true)
            {
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            }
            // Blue - ����
            else
            {
                Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
            }
        }
        else if (_isGuard == false)
        {
            // yellow - �и� ���� �� �ִ� ���� Ÿ�̹�
            if (_canBeParryied == true)
            {
                Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
            }
            // Red - ����
            else
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            }
        }

        DrawGizmos();
    }
}
