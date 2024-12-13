using UnityEngine;

/// <summary>
/// ���� ������ ���� �� ��� ���� ����� �����ϴ� Ŭ�����Դϴ�.
/// ������ ���� ���۰� ����, ����� �� �ִ� ������ ���� ���� ����մϴ�.
/// </summary>
public class MeleeWeapon : MonoBehaviour
{
    public SimpleDamager simpleDamager;
    public ParryDamager parryDamaer;
	public BoxColliderAdjuster bAdjuster;

    /// <summary>
    /// ������ ����(������)�� �����մϴ�.
    /// </summary>
    public void SetOwner(GameObject owner) => simpleDamager?.SetOwner(owner);

    /// <summary>
    /// ������ ������ �� ȣ��˴ϴ�.
    /// ���� �� SimpleDamager�� Ȱ��ȭ�ϰ�, ParryDamager�� ���� ���¸� Ȱ��ȭ�մϴ�.
    public void BeginAttack()
    {
        simpleDamager?.gameObject.SetActive(true);

        if (parryDamaer)
        {
            parryDamaer.inAttack = true;
        }
    }

    /// <summary>
    /// ������ ������ �� ȣ��˴ϴ�.
    /// ���� ���� �� SimpleDamager�� ��Ȱ��ȭ�ϰ�, ParryDamager�� ���� ���¸� ��Ȱ��ȭ�մϴ�.
    /// </summary>
    public void EndAttack()
    {
        simpleDamager?.gameObject.SetActive(false);

        if (parryDamaer)
        {
            parryDamaer.inAttack = false;
        }
    }

    /// <summary>
    /// �и�(�ݰ�) ���·� ������ �� �ִ� ���� �� ȣ��˴ϴ�.
    /// </summary>
    public void BeginCanBeParried()
    {
        parryDamaer?.BeginBeParried();
    }

    /// <summary>
    /// �и�(�ݰ�) ���¸� ������ �� ȣ��˴ϴ�.
    /// </summary>
    public void EndBeCanParried()
    {
        parryDamaer?.EndBeParried();
    }

    // -----
    private void Awake()
    {
        if (simpleDamager == null)
            simpleDamager = GetComponentInChildren<SimpleDamager>();
        if (parryDamaer == null)
            parryDamaer = GetComponentInChildren<ParryDamager>();
        bAdjuster = GetComponentInChildren<BoxColliderAdjuster>();

        simpleDamager?.gameObject.SetActive(false);
    }

}
