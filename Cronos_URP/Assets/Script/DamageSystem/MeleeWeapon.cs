using UnityEngine;
public class MeleeWeapon : MonoBehaviour
{
    private SimpleDamager _simpleDamager;
    private ParryDamager _parryDamaer;

    private void Awake()
    {
        _simpleDamager = GetComponentInChildren<SimpleDamager>();
        _parryDamaer = GetComponentInChildren<ParryDamager>();

        _simpleDamager?.gameObject.SetActive(false);
    }

    public void BeginAttack()
    {
        _simpleDamager?.gameObject.SetActive(true);
        _simpleDamager?.BeginAttack();
    }

    public void EndAttack()
    {
        _simpleDamager?.EndAttack();
        _simpleDamager?.gameObject.SetActive(false);
    }

    public void BeginCanBeParried()
    {
        _parryDamaer?.BeginBeParried();
    }

    public void EndBeCanParried()
    {
        _parryDamaer?.EndBeParried();
    }

}
