using UnityEngine;

/// <summary>
/// 근접 무기의 공격 및 방어 관련 기능을 관리하는 클래스입니다.
/// 무기의 공격 시작과 종료, 방어할 수 있는 상태의 설정 등을 담당합니다.
/// </summary>
public class MeleeWeapon : MonoBehaviour
{
    public SimpleDamager simpleDamager;
    public ParryDamager parryDamaer;
	public BoxColliderAdjuster bAdjuster;

    /// <summary>
    /// 무기의 주인(소유자)을 설정합니다.
    /// </summary>
    public void SetOwner(GameObject owner) => simpleDamager?.SetOwner(owner);

    /// <summary>
    /// 공격을 시작할 때 호출됩니다.
    /// 공격 시 SimpleDamager를 활성화하고, ParryDamager의 공격 상태를 활성화합니다.
    public void BeginAttack()
    {
        simpleDamager?.gameObject.SetActive(true);

        if (parryDamaer)
        {
            parryDamaer.inAttack = true;
        }
    }

    /// <summary>
    /// 공격을 종료할 때 호출됩니다.
    /// 공격 종료 시 SimpleDamager를 비활성화하고, ParryDamager의 공격 상태를 비활성화합니다.
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
    /// 패리(반격) 상태로 설정할 수 있는 시작 시 호출됩니다.
    /// </summary>
    public void BeginCanBeParried()
    {
        parryDamaer?.BeginBeParried();
    }

    /// <summary>
    /// 패리(반격) 상태를 종료할 때 호출됩니다.
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
