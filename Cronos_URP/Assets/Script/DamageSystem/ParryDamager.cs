using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �и�(�ݰ�) ����� ó���ϴ� Ŭ�����Դϴ�.
/// ������ ���� �� �и� ������ ���¸� Ȯ���ϰ�, �и� ���� �� �̺�Ʈ�� �߻���ŵ�ϴ�.
/// </summary>
public class ParryDamager : SimpleDamager
{
    [SerializeField]
    private bool _isGuard;
    [SerializeField]
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

    /// <summary>
    /// �и� ���¸� Ȯ���մϴ�. ��밡 �и� ������ ��������, �и� ������ üũ�մϴ�.
    /// </summary>
    /// <param name="other">�浹�� ������Ʈ</param>
    /// <returns>�и� ���� ����</returns>
    public bool ParryiedCheck(Collider other)
    {
        var otherParryDamager = other.GetComponent<ParryDamager>();

        // �и��� �� ���� ���ǵ�
        if (otherParryDamager == null ||
            otherParryDamager?._isParrying == false ||
            _canBeParryied == false)
        {
            return false;
        }

        // �и� ���� �� �̺�Ʈ �߻�
        beParryied.Invoke();
        otherParryDamager.parrying?.Invoke();

        return true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        if (_isGuard == false)
        {
            // yellow - �и� ���� �� �ִ� ���� Ÿ�̹�
            if (_canBeParryied == true)
            {
                Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
            }
            // Red - ����
            else if (inAttack == true)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            }
        }
        else if (_isGuard == true)
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

        DrawGizmos();
    }
}
